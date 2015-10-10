using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HelperSharp;
using KissSpecifications;
using SampleProject.Infrastructure.Framework.Globalization;

namespace SampleProject.Domain.Accounts.Specs
{
    /// <summary>
    /// Especificação referente a se uma senha de usuário é forte.
    /// </summary>
    public class PasswordMustBeStrongSpec : SpecificationBase<string>
    {
        #region Constants
        public const int MinLength = 8;
        public const int MinUppercase = 1;
        public const int MinSpecial = 1;
        public const int MinNumber = 1;
        public const int NonRepeatLastHistories = 20;
        #endregion

        #region Fields
        private int m_userId;
        private IUserPasswordService m_userPasswordService;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="PasswordMustBeStrongSpec" />.
        /// </summary>
        /// <param name="userId">O id do usuário.</param>
        /// <param name="userPasswordService">O serviço de senha de usuário.</param>
        public PasswordMustBeStrongSpec(int userId, IUserPasswordService userPasswordService)
        {
            m_userId = userId;
            m_userPasswordService = userPasswordService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Verifica se o alvo satisfaz a especificação.
        /// </summary>
        /// <param name="target">O alvo.</param>
        /// <returns>True se satisfaz a especificação, false no contrário.</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public override bool IsSatisfiedBy(string target)
        {
            // Tamanho mínimo.
            if (String.IsNullOrEmpty(target) || target.Length < MinLength)
            {
                NotSatisfiedReason = Texts.PasswordMustHaveAtLeastChars.With(MinLength);
                return false;
            }

            // Deve ter letras maiúsculas.
            if (target.Count(c => Char.IsUpper(c)) < MinUppercase)
            {
                NotSatisfiedReason = Texts.PasswordMustHaveAtLeastUppercaseChars.With(MinUppercase);
                return false;
            }

            // Deve ter caracteres especiais.
            if (target.Count(c => !Char.IsLetterOrDigit(c)) < MinSpecial)
            {
                NotSatisfiedReason = Texts.PasswordMustHaveAtLeastSpecialChars.With(MinSpecial);
                return false;
            }

            // Deve ter números.
            if (target.Count(c => Char.IsNumber(c)) < MinNumber)
            {
                NotSatisfiedReason = Texts.PasswordMustHaveAtLeastNumber.With(MinNumber);
                return false;
            }

            // Não deve repetir as últimas senhas.
            var histories = m_userPasswordService.GetLatestPasswordHistories(m_userId, NonRepeatLastHistories);

            var encryptedPassword = m_userPasswordService.Encrypt(target);

            if (histories.Any(h => h.Password.Equals(encryptedPassword, StringComparison.Ordinal)))
            {
                NotSatisfiedReason = Texts.PasswordMustBeDiffThanLastPasswords.With(NonRepeatLastHistories);
                return false;
            }

            return true;
        }
        #endregion
    }
}
