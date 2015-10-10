using SampleProject.Domain.Accounts;
using Skahal.Infrastructure.Framework.Repositories;

namespace SampleProject.Infrastructure.Repositories.Memory
{
    /// <summary>
    /// Implementação de IRepository&lt;Role&gt; em memória.
    /// <remarks>
    /// Deve ser utilizado somente para propósitos de teste.
    /// </remarks>
    /// </summary>
    public class MemoryRoleRepository : MemoryRepository<Role>, IRepository<Role>
    {
        #region Fields
        private static int s_lastKey;
        #endregion

        #region Methods
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MemoryRoleRepository"/>.
        /// </summary>
        /// <param name="unitOfWork">A implementação de Unit Of Work.</param>
        public MemoryRoleRepository(IUnitOfWork unitOfWork)
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
