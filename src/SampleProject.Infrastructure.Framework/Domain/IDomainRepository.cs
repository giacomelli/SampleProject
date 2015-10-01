using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Skahal.Infrastructure.Framework.Domain;
using Skahal.Infrastructure.Framework.Repositories;

namespace SampleProject.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Define a interface de um repositório para ser utilizado pelo domínio.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IDomainRepository<TEntity> : IRepository<TEntity> where TEntity : IAggregateRoot
    {
        /// <summary>
        /// Busca todas as instâncias da entidade.
        /// </summary>
        /// <param name="offset">O início do retorno.</param>
        /// <param name="limit">A quantidade máxima de instâncias.</param>
        /// <param name="filter">O filtro.</param>
        /// <param name="order">A função de ordenação.</param>
        /// <returns>O resultado da busca.</returns>
        IEnumerable<TEntity> FindAll(int offset, int limit, Expression<Func<TEntity, bool>> filter, Func<IEnumerable<TEntity>, IEnumerable<TEntity>> order);
    }
}
