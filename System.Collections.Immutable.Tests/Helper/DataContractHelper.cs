using System.IO;
using System.Runtime.Serialization;

namespace System.Collections.Immutable.Tests.Helper
{
    class DataContractHelper
    {
        public static byte[] Serialize<T>(T obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                var serializer = GetDataContractSerializer<T>();
                serializer.WriteObject(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }

        public static T Deserialize<T>(byte[] data)
        {
            using (var memoryStream = new MemoryStream(data))
            {
                var serializer = GetDataContractSerializer<T>();
                return (T)serializer.ReadObject(memoryStream);
            }
        }

#if  NETCOREAPP2_0

        public static DataContractSerializer GetDataContractSerializer<T>()
        {
            var serializer = new DataContractSerializer(typeof(T));
            serializer.SetSerializationSurrogateProvider(new ImmutableSurrogateProvider());
            return serializer;
        }

#elif NET45

        public static DataContractSerializer GetDataContractSerializer<T>()
        {
            var settings = new DataContractSerializerSettings();
            settings.DataContractSurrogate = new ImmutableSurrogateProvider();
            return new DataContractSerializer(typeof(T), settings);
        }

#endif

    }
}
