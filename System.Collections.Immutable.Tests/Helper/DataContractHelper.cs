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
                var serializer = new DataContractSerializer(typeof(T));
                serializer.SetSerializationSurrogateProvider(new ImmutableSurrogateProvider());
                serializer.WriteObject(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }

        public static T Deserialize<T>(byte[] data)
        {
            using (var memoryStream = new MemoryStream(data))
            {
                var serializer = new DataContractSerializer(typeof(T));
                serializer.SetSerializationSurrogateProvider(new ImmutableSurrogateProvider());
                return (T)serializer.ReadObject(memoryStream);
            }
        }
    }
}
