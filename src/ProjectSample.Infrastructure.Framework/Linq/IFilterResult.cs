using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace ProjectSample.Infrastructure.Framework.Linq
{
    /// <summary>
    /// Define a interface básica de um resultado de filtro.
    /// </summary>
    public interface IFilterResult
    {
        /// <summary>
        /// Obtém ou define o total de items localizados pelo filtro.
        /// </summary>
        long TotalCount { get; set; }

        /// <summary>
        /// Obtém as entidades.
        /// </summary>
        /// <returns>As entidades.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        IEnumerable GetEntities();

        /// <summary>
        /// Define as entidades.
        /// </summary>
        /// <param name="entities">As entidades.</param>
        void SetEntities(IEnumerable entities);
    }
}
