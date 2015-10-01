using System;
using System.Diagnostics.CodeAnalysis;
using SampleProject.Infrastructure.Framework.Globalization;
using SampleProject.Infrastructure.Framework.Runtime;

namespace SampleProject.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Carimbo de registro de uma ação.
    /// <remarks>
    /// Normalmente utilizado para marcar ações de criação e alteração de entidades.
    /// </remarks>
    /// </summary>
    public class ActionStamp
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ActionStamp"/>.
        /// </summary>
        public ActionStamp()
        {
            Date = DateTime.UtcNow;
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ActionStamp" />.
        /// </summary>
        /// <param name="date">A data.</param>
        public ActionStamp(DateTime date)
        {
            Date = date.ToUniversalTime();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém a data de quando foi realizada a ação.
        /// </summary>
        public DateTime Date { get; internal set; }

        /// <summary>
        /// Obtém o id do usuário que realizou a ação.
        /// </summary>
        public int UserId { get; internal set; }
        #endregion

        #region Methods
        /// <summary>
        /// Realiza o carimbo.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal void Stamp()
        {
            Stamp(RuntimeContext.Current.User.Id);
        }

        /// <summary>
        /// Realiza o carimbo.
        /// </summary>
        /// <param name="userId">O id do usuário.</param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal void Stamp(int userId)
        {
            if (userId == 0)
            {
                throw new ArgumentException(Texts.CanStampRecordWithNoUser);
            }

            Date = DateTime.UtcNow;
            UserId = userId;
        }

        /// <summary>
        /// Realiza o carimbo.
        /// </summary>
        /// <param name="date">A data.</param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal void Stamp(DateTime date)
        {
            Date = date;
            UserId = RuntimeContext.Current.User.Id;
        }
        #endregion
    }
}
