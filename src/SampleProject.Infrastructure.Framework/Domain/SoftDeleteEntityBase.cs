using System.Diagnostics.CodeAnalysis;

namespace SampleProject.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Classe base para entidade de domínio com deleção lógica.
    /// </summary>
    public abstract class SoftDeleteEntityBase : DomainEntityBase
    {
        #region Properties
        /// <summary>
        /// Obtém ou define um valor que indica se foi excluída.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Utilizada pelo EF.")]
        internal bool IsDeleted { get; set; }
        #endregion
    }
}
