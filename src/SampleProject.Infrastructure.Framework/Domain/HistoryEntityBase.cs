namespace SampleProject.Infrastructure.Framework.Domain
{
    #region Enum
    /// <summary>
    /// Estados de um histórico.
    /// </summary>
    public enum HistoryStatus
    {
        /// <summary>
        /// Ainda não foi revisado.
        /// </summary>
        NotReviewed,

        /// <summary>
        /// Foi aprovado.
        /// </summary>
        Approved,

        /// <summary>
        /// Foi reprovado.
        /// </summary>
        Disapproved
    }

    /// <summary>
    /// Tipos de históricos.
    /// </summary>
    public enum HistoryKind
    {
        /// <summary>
        /// Histórico de atualização.
        /// </summary>
        Update,

        /// <summary>
        /// Histórico de remoção.
        /// </summary>
        Remove
    }
    #endregion

    /// <summary>
    /// Classe base para entidade de histórico.
    /// </summary>
    /// <typeparam name="TTarget">O tipo da entidade que será alvo do histórico.</typeparam>
    public abstract class HistoryEntityBase<TTarget> : DomainEntityBase
        where TTarget : DomainEntityBase
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="HistoryEntityBase{TTarget}"/>.
        /// </summary>
        protected HistoryEntityBase()
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="HistoryEntityBase{TTarget}"/>.
        /// </summary>
        protected HistoryEntityBase(TTarget target, HistoryKind kind = Domain.HistoryKind.Update)
        {
            HistoryTargetId = target.Id;
            HistoryKind = kind;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define o estado do histórico.
        /// </summary>
        public virtual HistoryStatus HistoryStatus { get; set; }

        /// <summary>
        /// Obtém ou define o tipo do histórico.
        /// </summary>
        public HistoryKind HistoryKind { get; set; }

        /// <summary>
        /// Obtém ou define o id alvo do histórico.
        /// </summary>
        public int HistoryTargetId { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Cria o alvo do histórico.
        /// <remarks>Os dados mantidos no histórico são utilizados.</remarks>
        /// </summary>
        /// <returns>O alvo do histórico.</returns>
        public abstract TTarget CreateHistoryTarget();
        #endregion
    }
}
