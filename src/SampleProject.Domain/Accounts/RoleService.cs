using System;
using System.Linq.Expressions;
using Escrutinador.Extensions.KissSpecifications;
using KissSpecifications;
using KissSpecifications.Commons;
using SampleProject.Infrastructure.Framework.Domain;
using SampleProject.Infrastructure.Framework.Linq;
using SampleProject.Infrastructure.Framework.Runtime;
using Skahal.Infrastructure.Framework.Repositories;

namespace SampleProject.Domain.Accounts
{
    /// <summary>
    /// Implementação padrão de IRoleService.
    /// </summary>
    public class RoleService : DomainServiceBase<Role, IRepository<Role>>, IRoleService
    {
        #region Fields
        private IPermissionService m_permissionService;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="RoleService"/>.
        /// </summary>
        /// <param name="roleRepository">O repositório de papéis.</param>
        /// <param name="unitOfWork">O unit of work.</param>
        /// <param name="permissionService">O serviço de permissões.</param>
        public RoleService(IRepository<Role> roleRepository, IUnitOfWork unitOfWork, IPermissionService permissionService)
            : base(roleRepository, unitOfWork)
        {
            m_permissionService = permissionService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Salva a entidade informada
        /// </summary>
        /// <param name="entity">A entidade a ser salva.</param>
        public override void Save(Role entity)
        {
            base.Save(entity);

            foreach (var p in entity.Permissions)
            {
                m_permissionService.Save(p);
            }
        }

        /// <summary>
        /// Obtém o papel pelo nome informado.
        /// </summary>
        /// <param name="name">O nome do papel desejado.</param>
        /// <returns>O papel.</returns>
        public Role GetByName(string name)
        {
            return MainRepository.FindFirst(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Obtém todos os papéis que atendem ao filtro informado.
        /// </summary>
        /// <param name="offset">O início do índice de resultado.</param>
        /// <param name="limit">A quantidade de usuários no resultado.</param>
        /// <param name="filter">O filtro.</param>
        /// <returns>
        /// Os papéis.
        /// </returns>
        public FilterResult<Role> GetByName(int offset, int limit, string filter)
        {
            return GetByText(offset, limit, "Name", filter);
        }

        /// <summary>
        /// Obtém todos os entidades que atendem ao filtro informado.
        /// </summary>
        /// <param name="offset">O início do índice de resultado.</param>
        /// <param name="limit">A quantidade de entidades no resultado.</param>
        /// <param name="filter">O filtro.</param>
        /// <returns>as entidades.</returns>
        public override FilterResult<Role> Get(int offset, int limit, Expression<Func<Role, bool>> filter)
        {
            return GetAscending(offset, limit, filter, o => o.Name);
        }
    
        /// <summary>
        /// Obtém as especificações que devem ser atentidas ao salvar a entidade.
        /// </summary>
        /// <param name="entity">A entidade.</param>
        /// <returns>As especificações.</returns>
        protected override ISpecification<Role>[] GetSaveSpecifications(Role entity)
        {
            return new ISpecification<Role>[]
            {
                new MustComplyWithMetadataSpecification<Role>(),
                new MustHaveUniqueTextSpecification<Role>(t => t.Name, t => GetByName(t))
            };
        }
        #endregion
    }
}
