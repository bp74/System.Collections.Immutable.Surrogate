using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

namespace System.Collections.Immutable
{
    public class ImmutableSurrogateProvider : ISerializationSurrogateProvider
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

        public Type GetSurrogateType(Type targetType)
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
    }
}
