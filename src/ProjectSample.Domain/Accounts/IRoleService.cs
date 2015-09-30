using ProjectSample.Infrastructure.Framework.Domain;
using ProjectSample.Infrastructure.Framework.Linq;
using ProjectSample.Infrastructure.Framework.Runtime;

namespace ProjectSample.Domain.Accounts
{
    /// <summary>
    /// Define a interface para um serviço de papéis de usuário.
    /// </summary>
    public interface IRoleService : IDomainService<Role>
    {
        #region Methods
        /// <summary>
        /// Obtém todos os papéis que atendem ao filtro informado.
        /// </summary>
        /// <param name="offset">O início do índice de resultado.</param>
        /// <param name="limit">A quantidade de usuários no resultado.</param>
        /// <param name="filter">O filtro.</param>
        /// <returns>Os papéis.</returns>
        FilterResult<Role> GetByName(int offset, int limit, string filter);
        #endregion
    }
}
