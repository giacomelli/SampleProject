using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SampleProject.Infrastructure.Framework.UnitTests.Domain
{
    [TestClass]
    public class DomainEntityBaseTest
    {
        [TestMethod]
        public void GetHashCode_DiffEntities_Diff()
        {
            var entity1 = new DomainEntityBaseStub() { Id = 1 };
            var entity2 = new DomainEntityBaseStub() { Id = 2 };

            Assert.AreNotEqual(entity1.GetHashCode(), entity2.GetHashCode());
        }
    }
}
