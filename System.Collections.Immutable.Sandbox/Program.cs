using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Immutable.Classic
{
    class Program
    {
        [DataContract]
        class Test
        {
            [DataMember]
            public ImmutableArray<int> Array { get; set; } = ImmutableArray<int>.Empty;

            [DataMember]
            public ImmutableList<string> List { get; set; } = ImmutableList<string>.Empty;
        }

        static void Main(string[] args)
        {
            var test = new Test();
            test.Array = test.Array.Add(1).Add(2).Add(3);
            test.List = test.List.Add("one").Add("two").Add("three");

            var serialized = Serialize<Test>(test);
            var xml = Encoding.UTF8.GetString(serialized);
            var result = Deserialize<Test>(serialized);
        }

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
