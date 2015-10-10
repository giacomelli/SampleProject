using System.Diagnostics.CodeAnalysis;
using SampleProject.Domain.Accounts;
using Skahal.Infrastructure.Framework.Repositories;

[module: SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Scope = "namespace", Target = "SampleProject.Infrastructure.Repositories.Memory", MessageId = "Shared")]

namespace SampleProject.Infrastructure.Repositories.Memory
{
    /// <summary>
    /// Implementação de IRepository&lt;User&gt; em memória.
    /// <remarks>
    /// Deve ser utilizado somente para propósitos de teste.
    /// </remarks>
    /// </summary>
    public class MemoryUserRepository : MemoryRepository<User>, IRepository<User>
    {
        #region Fields
        private static int s_lastKey;
        #endregion

        #region Methods
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MemoryUserRepository"/>.
        /// </summary>
        /// <param name="unitOfWork">A implementação de Unit Of Work.</param>
        public MemoryUserRepository(IUnitOfWork unitOfWork)
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
