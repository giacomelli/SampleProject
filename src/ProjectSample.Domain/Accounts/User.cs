using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using ProjectSample.Infrastructure.Framework.Domain;
using Skahal.Infrastructure.Framework.Domain;

[module: SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Scope = "namespace", Target = "ProjectSample.Domain.Accounts", MessageId = "Shared")]

namespace ProjectSample.Domain.Accounts
{
    /// <summary>
    /// Representa um usuário.
    /// </summary>        
    public class User : DomainEntityBase, IAggregateRoot, INamedEntity
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="User"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Apenas inicialize a propriedade com a lista vazia.")]
        public User()
        {
            Enabled = true;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define o ID do usuário externo a qual esse usuário está vinculado.
        /// </summary>
        ////[Required]
        [Display(Order = 0)]
        [StringLength(48)]
        public string ExternalUserId { get; set; }

        /// <summary>
        /// Obtém ou define o nome.
        /// </summary>
        [StringLength(100, MinimumLength = 2)]
        [Display(Order = 1)]
        public string FullName { get; set; }

        /// <summary>
        /// Obtém ou define o e-mail.
        /// </summary>
        [StringLength(255, MinimumLength = 3)]
        [Display(Order = 2)]
        public string Email { get; set; }

        /// <summary>
        /// Obtém ou define o nome de usuário.
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 3)]
        [Display(Order = 3)]
        public string UserName { get; set; }

        /// <summary>
        /// Obtém ou define a senha.
        /// </summary>
        [StringLength(255)]
        public string Password { get; set; }

        /// <summary>
        /// Obtém ou define o ID papel.
        /// </summary>
        [Required]
        public int RoleId { get; set; }

        /// <summary>
        /// Obtém ou define o papel.
        /// </summary>
        [Display(Order = 5)]
        [Required]
        public virtual Role Role { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se está habilitado.
        /// </summary>
        [Display(Order = 7)]
        public bool Enabled { get; set; }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        string INamedEntity.Name
        {
            get { return FullName; }
        }

        #endregion

        #region Methods       
        /// <summary>
        /// Mantém os valores imutáveis se o usuário já existe.
        /// </summary>
        /// <param name="oldUser">A versão anterior do usuário.</param>
        internal void KeepImmutableValues(User oldUser)
        {
            if (!IsNew)
            {
                UserName = oldUser.UserName;
            }
        }
        #endregion
    }
}
