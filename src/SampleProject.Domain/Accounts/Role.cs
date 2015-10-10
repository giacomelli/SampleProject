using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HelperSharp;
using SampleProject.Infrastructure.Framework.Domain;
using SampleProject.Infrastructure.Framework.Runtime;
using Skahal.Infrastructure.Framework.Domain;

namespace SampleProject.Domain.Accounts
{
    /// <summary>
    /// Representa um papel.
    /// </summary>    
    public class Role : DomainEntityBase, IAggregateRoot
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="Role"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Apenas inicialize a propriedade com a lista vazia.")]
        public Role()
        {
            Permissions = new List<Permission>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define o nome.
        /// </summary>
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }

        /// <summary>
        /// Obtém ou define a descrição.
        /// </summary>        
        [StringLength(100)]
        public string Description { get; set; }

        /// <summary>
        /// Obtém ou define as permissões do papel.
        /// </summary>        
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Required]
        public virtual IList<Permission> Permissions { get; set; }
  
        /// <summary>
        /// Obtém ou define um valor que indica se as notificações via e-mail estão habilitadas.
        /// </summary>
        public bool EmailNotificationEnabled { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Verifica se o papel tem permissão de acesso na controller e action informadas.
        /// </summary>
        /// <param name="controllerName">O nome da controller.</param>
        /// <param name="actionName">O nome da action.</param>
        /// <returns>True pode acessar, false no contrário.</returns>
        public bool CanAccess(string controllerName, string actionName)
        {
            ExceptionHelper.ThrowIfNull("controllerName", controllerName);
            ExceptionHelper.ThrowIfNull("actionName", actionName);

            var result = false;

            if (Permissions != null)
            {
                result = Permissions.Any(
                    p => p.ControllerName.Equals(controllerName, StringComparison.OrdinalIgnoreCase)
                         && p.ActionName.Equals(actionName, StringComparison.OrdinalIgnoreCase));
            }

            return result;
        }
        #endregion
    }
}
