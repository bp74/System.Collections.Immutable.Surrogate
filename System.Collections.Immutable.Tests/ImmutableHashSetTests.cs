using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Immutable.Tests.Helper;
using System.Linq;
using System.Runtime.Serialization;

namespace System.Collections.Immutable.Tests
{
    [TestClass]
    public class ImmutableHashSetTests
    {
        [DataContract]
        class Test
        {
            [DataMember]
            public ImmutableHashSet<int> HashSet { get; set; } = ImmutableHashSet<int>.Empty;
        }

        //-----------------------------------------------------------------------------------------

        [TestMethod]
        public void ImmutableHashSet_SerializeAndDeserialize_CorrectResult()
        {
            // ARRANGE
            var objectUnderTest = new Test();
            objectUnderTest.HashSet = objectUnderTest.HashSet.Add(1);
            objectUnderTest.HashSet = objectUnderTest.HashSet.Add(2);
            objectUnderTest.HashSet = objectUnderTest.HashSet.Add(3);
            objectUnderTest.HashSet = objectUnderTest.HashSet.Add(1);

            // ACT
            var serialized = DataContractHelper.Serialize<Test>(objectUnderTest);
            var result = DataContractHelper.Deserialize<Test>(serialized);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.HashSet);
            Assert.IsInstanceOfType(result.HashSet, typeof(ImmutableHashSet<int>));
            CollectionAssert.AreEqual(new int[] { 1, 2, 3 }, result.HashSet.OrderBy(i => i).ToArray());
        }
    }
}
