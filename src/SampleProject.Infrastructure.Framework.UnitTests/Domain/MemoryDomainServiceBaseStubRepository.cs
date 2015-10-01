
using Skahal.Infrastructure.Framework.Repositories;
namespace SampleProject.Infrastructure.Framework.UnitTests.Domain
{
    public class MemoryDomainServiceBaseStubRepository : MemoryRepository<DomainEntityBaseStub>, IRepository<DomainEntityBaseStub>
    {
        #region Fields
        private static int s_lastKey;
        #endregion

        #region Methods
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MemoryDomainServiceBaseStubRepository"/>.
        /// </summary>
        /// <param name="unitOfWork">A implementação de Unit Of Work.</param>
        public MemoryDomainServiceBaseStubRepository(IUnitOfWork unitOfWork)
            : base(
            unitOfWork,
            u =>
            {
                return ++s_lastKey;
            })
        {
            s_lastKey = 0;
        }
        #endregion
    }
}
