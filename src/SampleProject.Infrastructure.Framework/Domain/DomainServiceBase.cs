using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using AutoFilter;
using Escrutinador.Extensions.KissSpecifications;
using HelperSharp;
using KissSpecifications;
using KissSpecifications.Commons;
using SampleProject.Infrastructure.Framework.Linq;
using Skahal.Infrastructure.Framework.Domain;
using Skahal.Infrastructure.Framework.Repositories;

namespace SampleProject.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Classe base para serviços de domínio.
    /// </summary>
    /// <typeparam name="TEntity">A entidade que o serviço trabalha primariamente.</typeparam>
    /// <typeparam name="TMainRepository">O repositório principal.</typeparam>
    public abstract class DomainServiceBase<TEntity, TMainRepository> : ServiceBase<TEntity, TMainRepository, IUnitOfWork>, IDomainService<TEntity>
        where TEntity : DomainEntityBase, IAggregateRoot
        where TMainRepository : IRepository<TEntity>
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DomainServiceBase{TEntity, TMainRepository}"/>.
        /// </summary>
        /// <param name="repository">O repositório.</param>
        /// <param name="unitOfWork">O unit of work.</param>
        protected DomainServiceBase(TMainRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém a entidade pelo id informado.
        /// </summary>
        /// <param name="id">O id da entidade desejada.</param>
        /// <returns>A instância da entidade.</returns>
        public TEntity GetById(int id)
        {
            return MainRepository.FindBy(id);
        }

        /// <summary>
        /// Obtém todos as entidades que atendem ao filtro informado.
        /// </summary>
        /// <param name="offset">O início do índice de resultado.</param>
        /// <param name="limit">A quantidade de entidades no resultado.</param>
        /// <param name="filter">O filtro.</param>
        /// <returns>As entidades.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public virtual FilterResult<TEntity> Get(int offset, int limit, Expression<Func<TEntity, bool>> filter)
        {
            return new FilterResult<TEntity>(
                MainRepository.FindAllAscending(offset, limit, filter, o => o.Id),
                MainRepository.CountAll(filter));
        }

        /// <summary>
        /// Conta as entidades que atendem ao filtro informado.
        /// </summary>
        /// <param name="filter">O filtro.</param>
        /// <returns>O total de entidades que atendem ao filtro.</returns>
        public virtual long Count(Expression<Func<TEntity, bool>> filter)
        {
            return MainRepository.CountAll(filter);
        }

        /// <summary>
        /// Conta todas as entidades.
        /// </summary>
        /// <returns>O total de entidades.</returns>
        public virtual long Count()
        {
            return MainRepository.CountAll();
        }

        /// <summary>
        /// Obtém todos os entidades que atendem ao filtro informado ordernados.
        /// </summary>
        /// <param name="offset">O início do índice de resultado.</param>
        /// <param name="limit">A quantidade de entidades no resultado.</param>
        /// <param name="filter">O filtro.</param>
        /// <param name="orderBy">A ordenação.</param>
        /// <typeparam name="TOrderByKey">O tipo da ordenação.</typeparam>
        /// <returns>as entidades.</returns>
        public virtual FilterResult<TEntity> GetAscending<TOrderByKey>(int offset, int limit, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderByKey>> orderBy)
        {
            return new FilterResult<TEntity>(
               MainRepository.FindAllAscending(offset, limit, filter, orderBy),
               MainRepository.CountAll(filter));
        }

        /// <summary>
        /// Obtém todos os entidades que atendem ao filtro informado ordernados.
        /// </summary>
        /// <param name="offset">O início do índice de resultado.</param>
        /// <param name="limit">A quantidade de entidades no resultado.</param>
        /// <param name="filter">O filtro.</param>
        /// <param name="orderBy">A ordenação.</param>
        /// <typeparam name="TOrderByKey">O tipo da ordenação.</typeparam>
        /// <returns>as entidades.</returns>
        public virtual FilterResult<TEntity> GetDescending<TOrderByKey>(int offset, int limit, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderByKey>> orderBy)
        {
            return new FilterResult<TEntity>(
               MainRepository.FindAllDescending(offset, limit, filter, orderBy),
               MainRepository.CountAll(filter));
        }

        /// <summary>
        /// Salva a entidade informada
        /// </summary>
        /// <param name="entity">A entidade a ser salva.</param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public virtual void Save(TEntity entity)
        {
            ExceptionHelper.ThrowIfNull("entity", entity);

            SpecService.Assert(
               entity,
               GetSaveSpecifications(entity));

            MainRepository[entity.Id] = entity;
        }

        /// <summary>
        /// Remove a entidade com o id informado.
        /// </summary>
        /// <param name="id">O id da entidade a ser removida.</param>
        public virtual void Remove(int id)
        {
            var entity = MainRepository.FindBy(id);

            SpecService.Assert(
                entity,
                new MustNotBeNullSpecification<TEntity>());

            SpecService.Assert(entity, GetRemoveSpecifications(entity));

            MainRepository.Remove(entity);
        }

        /// <summary>
        /// Obtém as especificações que devem ser atentidas ao salvar a entidade.
        /// </summary>
        /// <param name="entity">A entidade.</param>
        /// <returns>As especificações.</returns>
        protected virtual ISpecification<TEntity>[] GetSaveSpecifications(TEntity entity)
        {
            return new ISpecification<TEntity>[]
            {
                new MustComplyWithMetadataSpecification<TEntity>()
            };
        }

        /// <summary>
        /// Obtém as especificações que devem ser atentidas ao remover a entidade.
        /// </summary>
        /// <param name="entity">A entidade.</param>
        /// <returns>As especificações.</returns>
        protected virtual ISpecification<TEntity>[] GetRemoveSpecifications(TEntity entity)
        {
            return new ISpecification<TEntity>[0];
        }

        /// <summary>
        /// Obtém todos as entidades que atendem ao filtro informado.
        /// </summary>
        /// <param name="offset">O início do índice de resultado.</param>
        /// <param name="limit">A quantidade de usuários no resultado.</param>
        /// <param name="propertyName">O nome da propriedade.</param>
        /// <param name="propertyFilter">O filtro.</param>
        /// <returns>
        /// As entidades.
        /// </returns>
        protected FilterResult<TEntity> GetByText(int offset, int limit, string propertyName, string propertyFilter)
        {
            if (String.IsNullOrEmpty(propertyFilter))
            {
                return Get(offset, limit, f => true);
            }
            else
            {
                var autoFilter = new AutoFilterBuilder<TEntity>(new StringContainsIgnoreCaseAutoFilterStrategy(propertyName));

                return Get(
                    offset,
                    limit,
                    autoFilter.Build(propertyFilter));
            }
        }
        #endregion
    }
}
