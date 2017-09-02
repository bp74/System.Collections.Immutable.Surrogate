using System.CodeDom;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

namespace System.Collections.Immutable
{
#if NETSTANDARD2_0
    public class ImmutableSurrogateProvider : ISerializationSurrogateProvider
#elif NET45
    public class ImmutableSurrogateProvider : IDataContractSurrogate
#else
    public class ImmutableSurrogateProvider
#endif
    {
        private delegate object Activator(object arg);

        private static ConcurrentDictionary<Type, Activator> activators = new ConcurrentDictionary<Type, Activator>();
        private static ConcurrentDictionary<Type, Type> surrogateTypes = new ConcurrentDictionary<Type, Type>();
        private static ConcurrentDictionary<Type, Type> immutableTypes = new ConcurrentDictionary<Type, Type>();

        static ImmutableSurrogateProvider()
        {
            immutableTypes[typeof(ImmutableDictionary<,>)] = typeof(ImmutableDictionarySurrogate<,>);
            immutableTypes[typeof(ImmutableHashSet<>)] = typeof(ImmutableHashSetSurrogate<>);
            immutableTypes[typeof(ImmutableArray<>)] = typeof(ImmutableArraySurrogate<>);
            immutableTypes[typeof(ImmutableList<>)] = typeof(ImmutableListSurrogate<>);
        }

        //-----------------------------------------------------------------------------------------

#if NETSTANDARD2_0
        public Type GetSurrogateType(Type targetType)
#elif NET45
        public Type GetDataContractType(Type targetType)
#endif
        {
            Type surrogateType;

            if (surrogateTypes.TryGetValue(targetType, out surrogateType))
            {
                return surrogateType;
            }

            var targetTypeInfo = targetType.GetTypeInfo();

            if (targetTypeInfo.IsGenericType && immutableTypes.TryGetValue(targetType.GetGenericTypeDefinition(), out var type))
            {
                // get the type and the constructor of the generic surrogate type
                surrogateType = type.MakeGenericType(targetTypeInfo.GenericTypeArguments);
                var surrogateCtor = surrogateType.GetTypeInfo().DeclaredConstructors.FirstOrDefault(ci => ci.GetParameters().Length == 1);

                // compile a lambda function for the constructor to make it fast!
                var lambdaParam = Expression.Parameter(typeof(object));
                var targetParam = Expression.Convert(lambdaParam, targetType);
                var newInstance = Expression.New(surrogateCtor, targetParam);
                var activatorLambda = Expression.Lambda(typeof(Activator), newInstance, lambdaParam);
                var activatorCompiled = (Activator)activatorLambda.Compile();

                // store compiled lambda (activator) and surrogate type
                activators[surrogateType] = activatorCompiled;
                surrogateTypes[targetType] = surrogateType;
                return surrogateType;
            }

            return surrogateTypes[targetType] = targetType;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            return obj is ImmutableSurrogate ? ((ImmutableSurrogate)obj).ToImmutable() : obj;
        }

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            return activators.TryGetValue(targetType, out var activator) ? activator(obj) : obj;
        }

#if NET45

        public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            throw new NotImplementedException();
        }

        public object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            throw new NotImplementedException();
        }

        public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
        {
            throw new NotImplementedException();
        }

        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            throw new NotImplementedException();
        }

        public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
        {
            throw new NotImplementedException();
        }

#endif
    }
}
