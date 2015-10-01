using System;
using System.Linq.Expressions;
using SampleProject.Infrastructure.Framework.Linq;
using Skahal.Infrastructure.Framework.Domain;

namespace SampleProject.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Extensions methods para serviços de domínio.
    /// </summary>
    public static class DomainServiceExtensions
    {
        /// <summary>
        /// Obtém todos as entidades no intervalo informado.
        /// </summary>
        /// <param name="service">O serviço de domínio.</param>
        /// <param name="offset">O início do índice de resultado.</param>
        /// <param name="limit">A quantidade de entidades no resultado.</param>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <returns>As entidades.</returns>        
        public static FilterResult<TEntity> Get<TEntity>(this IDomainService<TEntity> service, int offset, int limit)
             where TEntity : DomainEntityBase, IAggregateRoot
        {
            return service.Get(offset, limit, null);
        }

        /// <summary>
        /// Obtém todos as entidades que atendem ao filtro informado.
        /// </summary>
        /// <param name="service">O serviço de domínio.</param>
        /// <param name="filter">O filtro.</param>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <returns>As entidades.</returns>
        public static FilterResult<TEntity> Get<TEntity>(this IDomainService<TEntity> service, Expression<Func<TEntity, bool>> filter)
            where TEntity : DomainEntityBase, IAggregateRoot
        {
            return service.Get(0, int.MaxValue, filter);
        }

        /// <summary>
        /// Obtém todos as entidades.
        /// </summary>
        /// <param name="service">O serviço de domínio.</param>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <returns>As entidades.</returns>
        public static FilterResult<TEntity> GetAll<TEntity>(this IDomainService<TEntity> service)
            where TEntity : DomainEntityBase, IAggregateRoot
        {
            return service.Get(0, int.MaxValue, null);
        }

        /// <summary>
        /// Obtém todos as entidades no intervalo informado ordernadas de forma ascendente.
        /// </summary>
        /// <param name="service">O serviço de domínio.</param>
        /// <param name="offset">O início do índice de resultado.</param>
        /// <param name="limit">A quantidade de entidades no resultado.</param>
        /// <param name="orderBy">A ordenação.</param>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <typeparam name="TOrderByKey">O tipo da ordenação.</typeparam>
        /// <returns>As entidades.</returns>
        public static FilterResult<TEntity> GetAscending<TEntity, TOrderByKey>(this IDomainService<TEntity> service, int offset, int limit, Expression<Func<TEntity, TOrderByKey>> orderBy)
            where TEntity : DomainEntityBase, IAggregateRoot
        {
            return service.GetAscending(offset, limit, null, orderBy);
        }

        /// <summary>
        /// Obtém todos as entidades ordernadas de forma ascendente.
        /// </summary>
        /// <param name="service">O serviço de domínio.</param>
        /// <param name="orderBy">A ordenação.</param>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <typeparam name="TOrderByKey">O tipo da ordenação.</typeparam>
        /// <returns>As entidades.</returns>
        public static FilterResult<TEntity> GetAscending<TEntity, TOrderByKey>(this IDomainService<TEntity> service, Expression<Func<TEntity, TOrderByKey>> orderBy)
            where TEntity : DomainEntityBase, IAggregateRoot
        {
            return service.GetAscending(0, int.MaxValue, null, orderBy);
        }

        /// <summary>
        /// Obtém todos as entidades no intervalo informado ordernadas de forma decrescente.
        /// </summary>
        /// <param name="service">O serviço de domínio.</param>
        /// <param name="offset">O início do índice de resultado.</param>
        /// <param name="limit">A quantidade de entidades no resultado.</param>
        /// <param name="orderBy">A ordenação.</param>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <typeparam name="TOrderByKey">O tipo da ordenação.</typeparam>
        /// <returns>As entidades.</returns>
        public static FilterResult<TEntity> GetDescending<TEntity, TOrderByKey>(this IDomainService<TEntity> service, int offset, int limit, Expression<Func<TEntity, TOrderByKey>> orderBy)
           where TEntity : DomainEntityBase, IAggregateRoot
        {
            return service.GetDescending(offset, limit, null, orderBy);
        }

        /// <summary>
        /// Obtém todos as entidades ordernadas de forma decrescente.
        /// </summary>
        /// <param name="service">O serviço de domínio.</param>
        /// <param name="orderBy">A ordenação.</param>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <typeparam name="TOrderByKey">O tipo da ordenação.</typeparam>
        /// <returns>As entidades.</returns>
        public static FilterResult<TEntity> GetDescending<TEntity, TOrderByKey>(this IDomainService<TEntity> service, Expression<Func<TEntity, TOrderByKey>> orderBy)
            where TEntity : DomainEntityBase, IAggregateRoot
        {
            return service.GetDescending(0, int.MaxValue, null, orderBy);
        }

        /// <summary>
        /// Obtém todos as entidades ordernadas de forma decrescente.
        /// </summary>
        /// <param name="service">O serviço de domínio.</param>
        /// <param name="filter">O filtro.</param>
        /// <param name="orderBy">A ordenação.</param>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <typeparam name="TOrderByKey">O tipo da ordenação.</typeparam>
        /// <returns>As entidades.</returns>
        public static FilterResult<TEntity> GetDescending<TEntity, TOrderByKey>(this IDomainService<TEntity> service, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderByKey>> orderBy)
            where TEntity : DomainEntityBase, IAggregateRoot
        {
            return service.GetDescending(0, int.MaxValue, filter, orderBy);
        }
    }
}
