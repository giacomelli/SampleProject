using System;
using System.Linq.Expressions;

namespace SampleProject.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Define a interface de componente que suporta pré-filtro.
    /// </summary>
    /// <typeparam name="TDomainEntity">O tipo de entidade.</typeparam>
    public interface IPreFilterable<TDomainEntity>
    {
        /// <summary>
        /// Obtém ou define o pré-filtro utilizado em todas as consultas.
        /// </summary>
        Expression<Func<TDomainEntity, bool>> PreFilter { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o pré-filtro deve ser ignorado.
        /// </summary>
        bool IgnorePreFilter { get; set; }
    }
}
