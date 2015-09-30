using System.Globalization;

namespace ProjectSample.Infrastructure.Framework.Runtime
{
    #region Enums
    /// <summary>
    /// Os possíveis ambientes de execução.
    /// </summary>
    public enum RuntimeEnvironment
    {
        /// <summary>
        /// Ambiente de desenvolvimento.
        /// </summary>
        Development,

        /// <summary>
        /// Ambiente de integração contínua.
        /// </summary>
        ContinousIntegration,

        /// <summary>
        /// Ambiente de teste.
        /// </summary>
        Test,

        /// <summary>
        /// Ambiente de homologação.
        /// </summary>
        Review,

        /// <summary>
        /// Ambiente de produção.
        /// </summary>
        Production
    }
    #endregion

    /// <summary>
    /// Responsável pelo contexto de execução.
    /// <remarks>A propriedade current deve ser configurada no bootstraper da aplicação.</remarks>
    /// </summary>
    public static class RuntimeContext
    {
        #region Constructors
        /// <summary>
        /// Inicia os membros estáticos da classe <see cref="RuntimeContext"/>.
        /// </summary>
        static RuntimeContext()
        {
            Current = new MemoryRuntimeContext()
            {
                Culture = new CultureInfo("pt-BR"),
                User = new MemoryRuntimeUser()
            };
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define o contexto de execução corrente.
        /// </summary>
        public static IRuntimeContext Current { get; set; }

        /// <summary>
        /// Obtém o ambiente atual de execução.
        /// </summary>
        public static RuntimeEnvironment Environment
        {
            get
            {
#if DEV
                return RuntimeEnvironment.Development;
#elif CI
                return RuntimeEnvironment.ContinousIntegration;
#elif TST
                return RuntimeEnvironment.Test;
#elif HLG
                return RuntimeEnvironment.Review;
#else
                return RuntimeEnvironment.Production;
#endif

            }
        }
        #endregion
    }
}
