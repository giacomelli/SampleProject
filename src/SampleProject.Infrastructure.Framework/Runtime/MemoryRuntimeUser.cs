using System;
using System.Collections.Generic;
using System.Linq;
using HelperSharp;
using SampleProject.Infrastructure.Framework.Domain;

namespace SampleProject.Infrastructure.Framework.Runtime
{
    /// <summary>
    /// Usuário de execução em memória.
    /// <remarks>Utilizado principalmente para questões de teste.</remarks>
    /// </summary>
    public class MemoryRuntimeUser : IRuntimeUser
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MemoryRuntimeUser"/>.
        /// </summary>
        public MemoryRuntimeUser()
        {
            ExternalUserIds = new List<string>();
            Regionals = new List<INamedEntity>();
            Subregionals = new List<INamedEntity>();
            Permissions = new List<IRuntimePermission>();
            SubstituingRoleIds = new List<int>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define o Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtém ou define o id externo.
        /// </summary>
        public IEnumerable<string> ExternalUserIds { get; set; }

        /// <summary>
        /// Obtém ou define o id externo do gerente.
        /// </summary>
        public string ManagerExternalIdImported { get; set; }

        /// <summary>
        /// Obtém ou define o id externo do diretor.
        /// </summary>
        public string DirectorExternalIdImported { get; set; }

        /// <summary>
        /// Obtém ou define os ids dos papéis que está substituindo.
        /// </summary>
        public IEnumerable<int> SubstituingRoleIds { get; set; }

        /// <summary>
        /// Obtém ou define o username.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Obtém ou define o nome completo.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Obtém ou define o e-mail.
        /// </summary> 
        public string Email { get; set; }

        /// <summary>
        /// Obtém um valor que indica se está autenticado.
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return Permissions.Any();
            }
        }
   
        /// <summary>
        /// Obtém ou define as regionais.
        /// </summary> 
        public IEnumerable<INamedEntity> Regionals { get; set; }

        /// <summary>
        /// Obtém ou define as subregionais.
        /// </summary> 
        public IEnumerable<INamedEntity> Subregionals { get; set; }

        /// <summary>
        /// Obtém ou define id do papel.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Obtém ou define as permissões.
        /// </summary> 
        public IEnumerable<IRuntimePermission> Permissions { get; set; }

        /// <summary>
        /// Obtém ou define o carimbo de última alteração do usuário.
        /// </summary>
        public ActionStamp Modified { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Verifica se pode acessar a controler e action informadas.
        /// </summary>
        /// <param name="controllerName">O nome da controller.</param>
        /// <param name="actionName">O nome da action.</param>
        /// <returns>
        /// True se pode acessar, false no contrário.
        /// </returns>
        public bool CanAccess(string controllerName, string actionName)
        {
            ExceptionHelper.ThrowIfNull("controllerName", controllerName);
            ExceptionHelper.ThrowIfNull("actionName", actionName);

            var result = false;

            result = Permissions.Any(
                    p => p.ControllerName.Equals(controllerName, StringComparison.OrdinalIgnoreCase)
                         && p.ActionName.Equals(actionName, StringComparison.OrdinalIgnoreCase));

            return result;
        }

        /// <summary>
        /// Verifica se o usuário possui qualquer um dos papéis informado.
        /// </summary>
        /// <param name="roleIds">Os ids dos papéis.</param>
        /// <returns>
        /// True se possui.
        /// </returns>
        public bool HasAnyRole(IEnumerable<int> roleIds)
        {
            return roleIds.Contains(RoleId) || roleIds.Intersect(SubstituingRoleIds).Any();
        }
        #endregion
    }
}
