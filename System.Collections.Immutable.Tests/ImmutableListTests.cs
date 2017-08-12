using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Immutable.Tests.Helper;
using System.Runtime.Serialization;

namespace System.Collections.Immutable.Tests
{
    [TestClass]
    public class ImmutableListTests
    {
        [DataContract]
        class Test
        {
            [DataMember]
            public ImmutableList<int> List { get; set; } = ImmutableList<int>.Empty;
        }

        //-----------------------------------------------------------------------------------------

        [TestMethod]
        public void ImmutableList_SerializeAndDeserialize_CorrectResult()
        {
            // ARRANGE
            var objectUnderTest = new Test();
            objectUnderTest.List = objectUnderTest.List.Add(1);
            objectUnderTest.List = objectUnderTest.List.Add(2);
            objectUnderTest.List = objectUnderTest.List.Add(3);

            // ACT
            var serialized = DataContractHelper.Serialize<Test>(objectUnderTest);
            var result = DataContractHelper.Deserialize<Test>(serialized);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.List);
            Assert.IsInstanceOfType(result.List, typeof(ImmutableList<int>));
            CollectionAssert.AreEqual(new int[] { 1, 2, 3 }, result.List);
        }
    }
}
