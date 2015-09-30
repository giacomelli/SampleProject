using System.ComponentModel.DataAnnotations;
using ProjectSample.Infrastructure.Framework.Domain;
using Skahal.Infrastructure.Framework.Domain;

namespace ProjectSample.Domain.Accounts
{
    /// <summary>
    /// Representa um histórico de senha do usuário.
    /// </summary>
    public class UserPasswordHistory : DomainEntityBase, IAggregateRoot
    {
        #region Constructors
        public UserPasswordHistory()
        {
        }

        public UserPasswordHistory(User user)
        {
            UserId = user.Id;
            Password = user.Password;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define o id do usuário.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Obtém ou define a senha
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Password { get; set; }
        #endregion
    }
}
