using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Immutable.Tests.Helper;
using System.Runtime.Serialization;

namespace System.Collections.Immutable.Tests
{
    [TestClass]
    public class ImmutableDictionaryTests
    {
        [DataContract]
        class Test
        {
            [DataMember]
            public ImmutableDictionary<int, string> Dictionary { get; set; } = ImmutableDictionary<int, string>.Empty;
        }

        //-----------------------------------------------------------------------------------------

        [TestMethod]
        public void ImmutableDictionary_SerializeAndDeserialize_CorrectResult()
        {
            // ARRANGE
            var objectUnderTest = new Test();
            objectUnderTest.Dictionary = objectUnderTest.Dictionary.Add(1, "one");
            objectUnderTest.Dictionary = objectUnderTest.Dictionary.Add(2, "two");
            objectUnderTest.Dictionary = objectUnderTest.Dictionary.Add(3, "three");

            // ACT
            var serialized = DataContractHelper.Serialize<Test>(objectUnderTest);
            var result = DataContractHelper.Deserialize<Test>(serialized);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Dictionary);
            Assert.IsInstanceOfType(result.Dictionary, typeof(ImmutableDictionary<int, string>));
            Assert.AreEqual("one", result.Dictionary[1]);
            Assert.AreEqual("two", result.Dictionary[2]);
            Assert.AreEqual("three", result.Dictionary[3]);
        }
    }
}
