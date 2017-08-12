using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Immutable.Tests.Helper;
using System.Runtime.Serialization;

namespace System.Collections.Immutable.Tests
{
    [TestClass]
    public class ImmutableArrayTests
    {
        [DataContract]
        class Test
        {
            [DataMember]
            public ImmutableArray<int> Array { get; set; } = ImmutableArray<int>.Empty;
        }

        //-----------------------------------------------------------------------------------------

        [TestMethod]
        public void ImmutableArray_SerializeAndDeserialize_CorrectResult()
        {
            // ARRANGE
            var objectUnderTest = new Test();
            objectUnderTest.Array = objectUnderTest.Array.Add(1);
            objectUnderTest.Array = objectUnderTest.Array.Add(2);
            objectUnderTest.Array = objectUnderTest.Array.Add(3);

            // ACT
            var serialized = DataContractHelper.Serialize<Test>(objectUnderTest);
            var result = DataContractHelper.Deserialize<Test>(serialized);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Array);
            Assert.IsInstanceOfType(result.Array, typeof(ImmutableArray<int>));
            CollectionAssert.AreEqual(new int[] { 1, 2, 3 }, result.Array);
        }
    }
}
