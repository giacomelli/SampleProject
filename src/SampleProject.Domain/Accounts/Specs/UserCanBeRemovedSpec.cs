using System.Diagnostics.CodeAnalysis;
using HelperSharp;
using KissSpecifications;
using SampleProject.Infrastructure.Framework.Globalization;

namespace SampleProject.Domain.Accounts.Specs
{
    /// <summary>
    /// Especificação referente a se um usuário pode ser removido.
    /// </summary>
    public class UserCanBeRemovedSpec : SpecificationBase<User>
    {
        #region Fields
        private int m_userId;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UserCanBeRemovedSpec"/>.
        /// </summary>
        /// <param name="userId">O id do usuário.</param>
        public UserCanBeRemovedSpec(int userId)
        {
            m_userId = userId;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Verifica se o alvo satisfaz a especificação.
        /// </summary>
        /// <param name="target">O alvo.</param>
        /// <returns>True se satisfaz a especificação, false no contrário.</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public override bool IsSatisfiedBy(User target)
        {
            if (target == null)
            {
                NotSatisfiedReason = Texts.UserWithIdDoNotExists.With(m_userId);
                return false;
            }

            if (target.Id == UserService.AdminUserId)
            {
                NotSatisfiedReason = Texts.SpecialUserCannotBeRemoved.With(UserService.AdminUserName);
                return false;
            }

            if (target.Id == UserService.BackgroundWokerUserId)
            {
                NotSatisfiedReason = Texts.SpecialUserCannotBeRemoved.With(UserService.BackgroundWokerUserName);
                return false;
            }

            return true;
        }
        #endregion
    }
}
