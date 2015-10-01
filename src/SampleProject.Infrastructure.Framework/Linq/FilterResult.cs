using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SampleProject.Infrastructure.Framework.Linq
{
    /// <summary>
    /// Representa o retorno de um filtro sobre entidades.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
    public sealed class FilterResult<TEntity> : IFilterResult
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="FilterResult{TEntity}"/>.
        /// </summary>
        /// <param name="entities">As entidades filtradas.</param>
        /// <param name="totalCount">The total count.</param>
        public FilterResult(IEnumerable<TEntity> entities, long totalCount)
        {
            Entities = entities;
            TotalCount = totalCount;
        }

        #region Properties
        /// <summary>
        /// Obtém ou define o número total de entidades que combinam com o filtro.
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// Obtém as entidades filtradas. Pode ser uma quantidade inferior a de TotalCount caso o filtro tenha sido feito pagindo.
        /// </summary>
        public IEnumerable<TEntity> Entities { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Traduz o resultado atual para uma versão utilizando objetos anônimos.
        /// </summary>
        /// <param name="toAnonymousFunc">A função que transformará cada item.</param>
        /// <returns>O resultado traduzido.</returns>
        public FilterResult<object> ToAnonymous(Func<TEntity, object> toAnonymousFunc)
        {
            var expandedEntities = new List<object>();

            foreach (var item in Entities)
            {
                expandedEntities.Add(toAnonymousFunc(item));
            }

            return new FilterResult<object>(expandedEntities, TotalCount);
        }

        IEnumerable IFilterResult.GetEntities()
        {
            return Entities;
        }

        void IFilterResult.SetEntities(IEnumerable entities)
        {
            Entities = entities.Cast<TEntity>();
        }

        #endregion
    }
}
