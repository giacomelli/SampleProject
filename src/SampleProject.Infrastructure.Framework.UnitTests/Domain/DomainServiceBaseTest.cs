using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skahal.Infrastructure.Framework.Repositories;

namespace SampleProject.Infrastructure.Framework.UnitTests.Domain
{
    [TestClass]
    public class DomainServiceBaseTest
    {
        [TestMethod]
        public void GetEntities_Args_Filtered()
        {
            var unitOfWork = new MemoryUnitOfWork();
            var repository = new MemoryDomainServiceBaseStubRepository(unitOfWork);
            repository.Add(new DomainEntityBaseStub());
            repository.Add(new DomainEntityBaseStub());
            unitOfWork.Commit();

            var target = new DomainServiceBaseStub(repository, unitOfWork);
            var actual = target.GetEntitiesStub();
            Assert.AreEqual(2, actual.TotalCount);
        }

        [TestMethod]
        public void GetEntitiesDescending_Args_Filtered()
        {
            var unitOfWork = new MemoryUnitOfWork();
            var repository = new MemoryDomainServiceBaseStubRepository(unitOfWork);
            repository.Add(new DomainEntityBaseStub());
            repository.Add(new DomainEntityBaseStub());
            unitOfWork.Commit();

            var target = new DomainServiceBaseStub(repository, unitOfWork);
            var actual = target.GetEntitiesDescendingStub();
            Assert.AreEqual(2, actual.TotalCount);
        }

        [TestMethod]
        public void Count_NoArgs_CountAllEntities()
        {
            var unitOfWork = new MemoryUnitOfWork();
            var repository = new MemoryDomainServiceBaseStubRepository(unitOfWork);
            repository.Add(new DomainEntityBaseStub());
            repository.Add(new DomainEntityBaseStub());
            unitOfWork.Commit();

            var target = new DomainServiceBaseStub(repository, unitOfWork);
            var actual = target.Count();
            Assert.AreEqual(2, actual);
        }
    }
}
