using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using ProjectSample.Infrastructure.Framework.Linq;
using Skahal.Infrastructure.Framework.Domain;

namespace ProjectSample.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Define a interface básica de um serviço de domínio.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IDomainService<TEntity> where TEntity : DomainEntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém todos as entidades que atendem ao filtro informado.
        /// </summary>
        /// <param name="offset">O início do índice de resultado.</param>
        /// <param name="limit">A quantidade de entidades no resultado.</param>
        /// <param name="filter">O filtro.</param>
        /// <returns>As entidades.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get")]
        FilterResult<TEntity> Get(int offset, int limit, Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Obtém todos as entidades que atendem ao filtro informado ordernado.
        /// </summary>
        /// <param name="offset">O início do índice de resultado.</param>
        /// <param name="limit">A quantidade de entidades no resultado.</param>
        /// <param name="filter">O filtro.</param>
        /// <param name="orderBy">A ordenação.</param>
        /// <typeparam name="TOrderByKey">O tipo da ordenação.</typeparam>
        /// <returns>As entidades.</returns>
        FilterResult<TEntity> GetAscending<TOrderByKey>(int offset, int limit, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderByKey>> orderBy);

        /// <summary>
        /// Obtém a entidade pelo id informado.
        /// </summary>
        /// <param name="id">O id da entidade desejada.</param>
        /// <returns>A instância da entidade.</returns>
        TEntity GetById(int id);

        /// <summary>
        /// Obtém todos os entidades que atendem ao filtro informado ordernados.
        /// </summary>
        /// <param name="offset">O início do índice de resultado.</param>
        /// <param name="limit">A quantidade de entidades no resultado.</param>
        /// <param name="filter">O filtro.</param>
        /// <param name="orderBy">A ordenação.</param>
        /// <typeparam name="TOrderByKey">O tipo da ordenação.</typeparam>
        /// <returns>as entidades.</returns>
        FilterResult<TEntity> GetDescending<TOrderByKey>(int offset, int limit, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderByKey>> orderBy);

        /// <summary>
        /// Remove a entidade com o id informado.
        /// </summary>
        /// <param name="id">O id da entidade a ser removida.</param>
        void Remove(int id);

        /// <summary>
        /// Salva a entidade informada
        /// </summary>
        /// <param name="entity">A entidade a ser salva.</param>
        void Save(TEntity entity);
    }
}
