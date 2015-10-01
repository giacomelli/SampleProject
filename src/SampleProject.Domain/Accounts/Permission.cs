using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using SampleProject.Infrastructure.Framework.Domain;
using SampleProject.Infrastructure.Framework.Runtime;
using Skahal.Infrastructure.Framework.Domain;

namespace SampleProject.Domain.Accounts
{
    /// <summary>
    /// Representa uma permissão no sistema.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class Permission : DomainEntityBase, IAggregateRoot, IRuntimePermission
    {
        #region Properties
        /// <summary>
        /// Obtém ou define o nome da controller relacionada a permissão.
        /// </summary>
        [StringLength(50, MinimumLength = 1)]
        public string ControllerName { get; set; }

        /// <summary>
        /// Obtém ou define o nome da controller referenciada relacionada a permissão.
        /// </summary>
        [StringLength(50)]
        public string ReferencedControllerName { get; set; }

        /// <summary>
        /// Obtém ou define o nome da action relacionada a permissão.
        /// </summary>
        [StringLength(50, MinimumLength = 1)]
        public string ActionName { get; set; }

        /// <summary>
        /// Obtém ou define o nome da action referencidada relacionada a permissão.
        /// </summary>
        [StringLength(50)]
        public string ReferencedActionName { get; set; }

        #endregion
    }
}
