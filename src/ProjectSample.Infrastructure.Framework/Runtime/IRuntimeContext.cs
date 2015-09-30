using System.Globalization;

namespace ProjectSample.Infrastructure.Framework.Runtime
{
    /// <summary>
    /// Define a interface de um contexto de execução.
    /// </summary>
    public interface IRuntimeContext
    {
        #region Properties
        /// <summary>
        /// Obtém o usuário corrente.
        /// </summary>
        IRuntimeUser User { get; }

        /// <summary>
        /// Obtém cultura.
        /// </summary>
        CultureInfo Culture { get; }
        #endregion
    }
}
