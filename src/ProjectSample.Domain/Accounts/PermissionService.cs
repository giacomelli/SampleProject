using System;
using System.Diagnostics.CodeAnalysis;
using HelperSharp;
using ProjectSample.Infrastructure.Framework.Domain;
using Skahal.Infrastructure.Framework.Repositories;

namespace ProjectSample.Domain.Accounts
{
    /// <summary>
    /// Serviço de domínio para permissões.
    /// </summary>
    public class PermissionService : DomainServiceBase<Permission, IRepository<Permission>>, IPermissionService
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="PermissionService"/>.
        /// <param name="mainRepository">O repositório principal.</param>
        /// <param name="unitOfWork">A implementação de Unit Of Work.</param>
        /// </summary>
        public PermissionService(IRepository<Permission> mainRepository, IUnitOfWork unitOfWork)
            : base(mainRepository, unitOfWork)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Salva a permissão.
        /// </summary>
        /// <param name="entity">A permissão a ser salva.</param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public override void Save(Permission entity)
        {
            ExceptionHelper.ThrowIfNull("entity", entity);

            var oldPermission = MainRepository.FindFirst(
                p => p.ControllerName.Equals(entity.ControllerName, StringComparison.OrdinalIgnoreCase)
                && p.ActionName.Equals(entity.ActionName, StringComparison.OrdinalIgnoreCase));

            if (oldPermission == null)
            {
                base.Save(entity);
            }
            else
            {
                entity.Id = oldPermission.Id;
            }
        }
        #endregion
    }
}
