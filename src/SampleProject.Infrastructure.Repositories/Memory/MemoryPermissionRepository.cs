using SampleProject.Domain.Accounts;
using Skahal.Infrastructure.Framework.Repositories;

namespace SampleProject.Infrastructure.Repositories.Memory
{
    /// <summary>
    /// Implementação de IRepository&lt;Permission&gt; em memória.
    /// <remarks>
    /// Deve ser utilizado somente para propósitos de teste.
    /// </remarks>
    /// </summary>
    public class MemoryPermissionRepository : MemoryRepository<Permission>, IRepository<Permission>
    {
        #region Fields
        private static int s_lastKey;
        #endregion

        #region Methods
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MemoryPermissionRepository"/>.
        /// </summary>
        /// <param name="unitOfWork">A implementação de Unit Of Work.</param>
        public MemoryPermissionRepository(IUnitOfWork unitOfWork)
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
