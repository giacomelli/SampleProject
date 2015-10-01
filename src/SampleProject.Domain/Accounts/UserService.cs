using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Escrutinador.Extensions.KissSpecifications;
using HelperSharp;
using KissSpecifications;
using KissSpecifications.Commons;
using SampleProject.Domain.Accounts.Specs;
using SampleProject.Infrastructure.Framework.Commons.Specs;
using SampleProject.Infrastructure.Framework.Domain;
using SampleProject.Infrastructure.Framework.Linq;
using SampleProject.Infrastructure.Framework.Runtime;
using Skahal.Infrastructure.Framework.Repositories;

namespace SampleProject.Domain.Accounts
{
    /// <summary>
    /// Implementação padrão de IUserService relativo a usuários.
    /// </summary>
    public class UserService
        : DomainServiceBase<User, IRepository<User>>, IUserService
    {
        #region Constants
        /// <summary>
        /// O login do usuário administrador.
        /// </summary>
        public static readonly string AdminUserName = "admin";

        /// <summary>
        /// O id do usuário administrador.
        /// </summary>
        public static readonly int AdminUserId = 1;

        /// <summary>
        /// O login do usuário utilizado pelo BackgroundWorker.
        /// </summary>
        public static readonly string BackgroundWokerUserName = "backgroundworker";

        /// <summary>
        /// O id do usuário utilizado pelo BackgroundWorker.
        /// </summary>
        public static readonly int BackgroundWokerUserId = 2;
        #endregion

        #region Fields
        private IRoleService m_roleService;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UserService"/>.
        /// </summary>
        /// <param name="userRepository">O repositório de usuários.</param>
        /// <param name="unitOfWork">O unit of work.</param>
        /// <param name="roleService">O serviço de papéis.</param>
        public UserService(IRepository<User> userRepository, IUnitOfWork unitOfWork, IRoleService roleService)
            : base(userRepository, unitOfWork)
        {
            m_roleService = roleService;
        }
        #endregion

        #region Public methods

        /// <summary>
        /// Obtém os usuários que atendem ao filtro especificado.
        /// </summary>
        /// <param name="offset">O início do índice de resultado.</param>
        /// <param name="limit">A quantidade de usuários no resultado.</param>        
        /// <param name="filter">O filtro.</param>
        /// <returns>Os usuários.</returns>
        public FilterResult<User> GetByFilter(int offset, int limit, UserFilter filter)
        {
            filter.Sanitize();

            var activeStatus = filter.UserActiveStatus.ToArray<bool>();
            var roleIds = filter.RoleIds.ToArray<int>();
            var hasStatus = activeStatus.Length > 0;
            var hasRoles = roleIds.Length > 0;

            Expression<Func<User, bool>> filterExpression = u => (null == filter.UserName || (null != u.UserName && u.UserName.ToLower().Contains(filter.UserName))) &&
                     (null == filter.FullName || (null != u.FullName && u.FullName.ToLower().Contains(filter.FullName))) &&
                     (null == filter.ExternalId || (null != u.ExternalUserId && u.ExternalUserId.ToLower().Contains(filter.ExternalId))) &&
                     (!hasStatus || activeStatus.Contains(u.Enabled)) &&
                     (!hasRoles || roleIds.Contains(u.Role.Id));

            var users = MainRepository.FindAllAscending(
                offset,
                limit,
                filterExpression,
                u => u.FullName);

            var count = MainRepository.CountAll(filterExpression);

            return new FilterResult<User>(users, count);
        }

        /// <summary>
        /// Obtém os usuários que possuem o filtro de nome completo.
        /// </summary>
        /// <param name="offset">O início do índice de resultado.</param>
        /// <param name="limit">A quantidade de usuários no resultado.</param>        
        /// <param name="filter">O filtro.</param>
        /// <returns>Os usuários.</returns>
        public FilterResult<User> GetByFullName(int offset, int limit, string filter)
        {
            return GetByText(offset, limit, "FullName", filter);
        }       

        /// <summary>
        /// Salva a entidade informada
        /// </summary>
        /// <param name="entity">A entidade a ser salva.</param>
        public override void Save(User entity)
        {
            ExceptionHelper.ThrowIfNull("entity", entity);

            entity.Role = m_roleService.GetById(entity.RoleId);

            var oldUser = GetById(entity.Id);        

            entity.KeepImmutableValues(oldUser);

            base.Save(entity);

            entity.Role = m_roleService.GetById(entity.Role.Id);
        }

        /// <summary>
        /// Obtém o usuário pelo nome do usuário.
        /// </summary>
        /// <param name="userName">O nome de usuário.</param>
        /// <returns>
        /// O usuário.
        /// </returns>
        public User GetByUserName(string userName)
        {
            ExceptionHelper.ThrowIfNullOrEmpty("userName", userName);

            return MainRepository.FindFirst(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Obtém o usuário pelo e-mail.
        /// </summary>
        /// <param name="email">O e-mail do usuário.</param>
        /// <returns>O usuário.</returns>
        public User GetByEmail(string email)
        {
            ExceptionHelper.ThrowIfNullOrEmpty("email", email);

            return MainRepository.FindFirst(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Obtém o usuário pelo nome completo do usuário.
        /// </summary>
        /// <param name="fullName">O nome completo do usuário.</param>
        /// <returns>
        /// O usuário.
        /// </returns>
        public User GetByFullName(string fullName)
        {
            ExceptionHelper.ThrowIfNullOrEmpty("fullName", fullName);

            return MainRepository.FindFirst(u => fullName.Equals(u.FullName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Obtém todos os entidades que atendem ao filtro informado.
        /// </summary>
        /// <param name="offset">O início do índice de resultado.</param>
        /// <param name="limit">A quantidade de entidades no resultado.</param>
        /// <param name="filter">O filtro.</param>
        /// <returns>as entidades.</returns>
        public override FilterResult<User> Get(int offset, int limit, Expression<Func<User, bool>> filter)
        {
            return GetAscending(offset, limit, filter, o => o.FullName);
        }

        /// <summary>
        /// Obtém o usuário pelo id do usuário externo.
        /// </summary>
        /// <param name="externalUserId">O id do usuário externo.</param>
        /// <returns>O usuário, se existir.</returns>
        public User GetByExternalUserId(string externalUserId)
        {
            return MainRepository.FindFirst(f => f.ExternalUserId == externalUserId);
        }     

        /// <summary>
        /// Verifica se uma notificação pode ser enviada para o e-mail informado.
        /// </summary>
        /// <param name="email">O e-mail de destino.</param>
        /// <returns>True se pode enviar a notificação.</returns>
        public bool CanSendEmailNotificationTo(string email)
        {
            var user = GetByEmail(email);

            return user != null
                && user.Enabled
                && user.Role.EmailNotificationEnabled;
        }
        #endregion

        #region Protected methods
        /// <summary>
        /// Obtém as especificações que devem ser atentidas ao salvar a entidade.
        /// </summary>
        /// <param name="entity">A entidade.</param>
        /// <returns>As especificações.</returns>
        protected override ISpecification<User>[] GetSaveSpecifications(User entity)
        {
            return new ISpecification<User>[]
            {
                new MustComplyWithMetadataSpecification<User>(),
                new MustHaveUniqueTextSpecification<User>(t => t.FullName, t => GetByFullName(t)),
                new MustHaveUniqueTextSpecification<User>(t => t.UserName, t => GetByUserName(t)),
                new MustHaveUniqueValueSpec<User, string>(t => t.Email, t => GetByEmail(t)),
                new MustHaveUniqueValueSpec<User, string>(t => t.ExternalUserId, t => GetByExternalUserId(t)),
                new MustNotHaveNullOrDefaultPropertySpecification<User>(t => t.Role),
                new MustExistsSpecification<User, Role>(t => t.Role, (r) => r != null && m_roleService.GetById(r.Id) != null)
            };
        }

        /// <summary>
        /// Obtém as especificações que devem ser atentidas ao remover a entidade.
        /// </summary>
        /// <param name="entity">A entidade.</param>
        /// <returns>As especificações.</returns>
        protected override ISpecification<User>[] GetRemoveSpecifications(User entity)
        {
            return new ISpecification<User>[] { new UserCanBeRemovedSpec(entity.Id) };
        }
        #endregion
    }
}
