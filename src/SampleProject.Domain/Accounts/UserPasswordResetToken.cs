using System;
using System.ComponentModel.DataAnnotations;
using SampleProject.Infrastructure.Framework.Domain;
using Skahal.Infrastructure.Framework.Domain;

namespace SampleProject.Domain.Accounts
{
    /// <summary>
    /// Representa um token para redefinição de senha do usuário.
    /// </summary>
    public class UserPasswordResetToken : DomainEntityBase, IAggregateRoot
    {
        #region Fields
        private bool m_expired;
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define o id do usuário para qual foi gerado o token.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Obtém ou define o código do token.
        /// </summary>
        [Required]
        public Guid Code { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o token foi enviado ao usuário.
        /// </summary>
        public bool Sent { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o token foi utilizado para resetar um senha.
        /// </summary>
        public bool Used { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o usuário foi notificado do uso do token.
        /// </summary>
        public bool NotifiedUse { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se está expirado.
        /// </summary>
        public bool Expired
        {
            get
            {
                return m_expired || Created.Date.AddMinutes(UserPasswordService.PasswordResetTokenMinutesTimeout) < DateTime.UtcNow;
            }

            set
            {
                m_expired = value;
            }
        }
        #endregion
    }
}
