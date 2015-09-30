namespace ProjectSample.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Define a interface de uma entidade que possui nome.
    /// </summary>
    public interface INamedEntity : IEntity
    {
        #region Properties
        /// <summary>
        /// Obtém o nome.
        /// </summary>
        string Name { get; }
        #endregion
    }
}
