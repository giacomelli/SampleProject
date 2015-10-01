using System;

namespace SampleProject.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Define a interface de um container de entidade.
    /// </summary>
    public interface IEntityContainer
    {
        /// <summary>
        /// Obtém o tipo da entidade.
        /// </summary>
        Type EntityType { get; }
    }
}
