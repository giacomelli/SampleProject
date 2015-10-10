using SampleProject.Infrastructure.Framework.Domain;
using SampleProject.Infrastructure.Framework.Linq;
using Skahal.Infrastructure.Framework.Repositories;

namespace SampleProject.Infrastructure.Framework.UnitTests.Domain
{
    public class DomainServiceBaseStub : DomainServiceBase<DomainEntityBaseStub, IRepository<DomainEntityBaseStub>>
    {
        public DomainServiceBaseStub(IRepository<DomainEntityBaseStub> repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public FilterResult<DomainEntityBaseStub> GetEntitiesStub()
        {
            return Get(0, int.MaxValue, f => true);
        }

        public FilterResult<DomainEntityBaseStub> GetEntitiesDescendingStub()
        {
            return GetDescending(0, int.MaxValue, f => true, o => o.Id);
        }
    }
}
