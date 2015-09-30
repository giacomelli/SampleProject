using System.Collections.Generic;
using ProjectSample.Infrastructure.Framework.Domain;

namespace ProjectSample.Infrastructure.Framework.Runtime
{   
    /// <summary>
    /// Define a interface de usuário que pode executar a aplicação.
    /// </summary>
    public interface IRuntimeUser
    {
        #region Properties
        /// <summary>
        /// Obtém o Id.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Obtém o id externo.
        /// </summary>
        IEnumerable<string> ExternalUserIds { get; }

        /// <summary>
        /// Obtém o id externo do gerente.
        /// </summary>
        string ManagerExternalIdImported { get; }

        /// <summary>
        /// Obtém o id externo do diretor.
        /// </summary>
        string DirectorExternalIdImported { get; }

        /// <summary>
        /// Obtém o username.
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Obtém o nome completo.
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Obtém o e-mail.
        /// </summary> 
        string Email { get; }

        /// <summary>
        /// Obtém um valor que indica se está autenticado.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Obtém as regionais.
        /// </summary> 
        IEnumerable<INamedEntity> Regionals { get; }

        /// <summary>
        /// Obtém as subregionais.
        /// </summary> 
        IEnumerable<INamedEntity> Subregionals { get; }

        /// <summary>
        /// Obtém o id do papel.
        /// </summary>
        int RoleId { get; }

        /// <summary>
        /// Obtém as permissões.
        /// </summary> 
        IEnumerable<IRuntimePermission> Permissions { get; }

        /// <summary>
        /// Obtém o carimbo de última alteração do usuário.
        /// </summary>
        ActionStamp Modified { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Verifica se pode acessar a controler e action informadas.
        /// </summary>
        /// <param name="controllerName">O nome da controller.</param>
        /// <param name="actionName">O nome da action.</param>
        /// <returns>True se pode acessar, false no contrário.</returns>
        bool CanAccess(string controllerName, string actionName);

        /// <summary>
        /// Verifica se o usuário possui qualquer um dos papéis informado.
        /// </summary>
        /// <param name="roleIds">Os ids dos papéis.</param>
        /// <returns>True se possui.</returns>
        bool HasAnyRole(IEnumerable<int> roleIds);
        #endregion
    }
}
