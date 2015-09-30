using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using HelperSharp;
using ProjectSample.Domain.Accounts.Specs;
using ProjectSample.Infrastructure.Framework.Domain;
using ProjectSample.Infrastructure.Framework.Globalization;
using Skahal.Infrastructure.Framework.Repositories;

namespace ProjectSample.Domain.Accounts
{
    /// <summary>
    /// Serviço de domínio para senha do usuário.
    /// </summary>
    public class UserPasswordService : IUserPasswordService
    {
        #region Fields
        private IRepository<UserPasswordResetToken> m_tokenRepository;
        private IRepository<UserPasswordHistory> m_historyRepository;
        private IUserService m_userService;
        #endregion

        #region Constructors
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static UserPasswordService()
        {
            PasswordResetTokenMinutesTimeout = 60;
            PasswordExpirationDays = 90;
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UserPasswordService"/>.
        /// </summary>
        /// <param name="tokenRepository">O repositório de token de reset de senha.</param>
        /// <param name="historyRepository">O repositório de histórico de senha.</param>
        /// <param name="userService">O serviço de usuários.</param>
        public UserPasswordService(IRepository<UserPasswordResetToken> tokenRepository, IRepository<UserPasswordHistory> historyRepository, IUserService userService)
        {
            m_tokenRepository = tokenRepository;
            m_historyRepository = historyRepository;
            m_userService = userService;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define o timeout, em minutos, para expirar um token para redefinição de senha do usuário.
        /// </summary>        
        public static int PasswordResetTokenMinutesTimeout { get; set; }

        /// <summary>
        /// Obtém ou define o número de dias para expirar uma senha.
        /// </summary>    
        public static int PasswordExpirationDays { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Realiza a geração de um token para redefinição de senha do usuário com o e-mail informado.
        /// </summary>
        /// <param name="email">O e-mail do usuário.</param>
        /// <returns>O token gerado.</returns>
        public UserPasswordResetToken GeneratePasswordResetToken(string email)
        {
            ExceptionHelper.ThrowIfNull("email", email);

            var user = m_userService.GetByEmail(email);

            if (user == null)
            {
                throw new ArgumentException(Texts.ThereIsNoUserWithThisEmail);
            }

            var oldToken = m_tokenRepository.FindFirst(t => t.UserId.Equals(user.Id));

            if (oldToken != null)
            {
                m_tokenRepository.Remove(oldToken);
            }

            var newToken = new UserPasswordResetToken()
            {
                UserId = user.Id,
                Code = Guid.NewGuid()
            };

            m_tokenRepository.Add(newToken);

            return newToken;
        }

        /// <summary>
        /// Reseta a senha do usuário.
        /// </summary>
        /// <param name="tokenCode">O código do token que será utilizado para resetar a senha.</param>
        /// <param name="newPassword">A nova senha.</param>
        /// <returns>
        /// O usuário com a senha resetada.
        /// </returns>        
        public User ResetPassword(Guid tokenCode, string newPassword)
        {
            ExceptionHelper.ThrowIfNull("tokenCode", tokenCode);
            ExceptionHelper.ThrowIfNull("newPassword", newPassword);

            var token = m_tokenRepository.FindFirst(t => t.Code.Equals(tokenCode) && !t.Used);

            if (token == null || token.Expired)
            {
                throw new ArgumentException(Texts.UserPasswordResetTokenExpired);
            }

            var user = m_userService.GetById(token.UserId);

            ChangePassword(user, newPassword);

            token.Used = true;
            m_tokenRepository[token.Id] = token;

            return user;
        }

        /// <summary>
        /// Altera a senha do usuário.
        /// </summary>
        /// <param name="userId">O id do usuário.</param>
        /// <param name="oldPassword">A senha antiga.</param>
        /// <param name="newPassword">A nova senha.</param>
        public void ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var user = m_userService.GetById(userId);

            if (!String.Equals(Encrypt(oldPassword), user.Password, StringComparison.Ordinal))
            {
                throw new InvalidOperationException(Texts.OldPasswordDoesNotMatch);
            }

            ChangePassword(user, newPassword);
        }

        /// <summary>
        /// Obtém os tokens de reset de senha que ainda não foram enviados aos usuários.
        /// </summary>
        /// <returns>Os tokens não enviados.</returns>
        public IEnumerable<UserPasswordResetToken> GetPasswordResetTokensNotSent()
        {
            return m_tokenRepository.FindAll(f => !f.Sent && !f.Used && !f.Expired);
        }

        /// <summary>
        /// Marca o token de reset de senha como enviado.
        /// </summary>
        /// <param name="id">O id do token.</param>
        public void MarkPasswordResetTokenAsSent(int id)
        {
            var token = m_tokenRepository.FindBy(id);

            if (token != null)
            {
                token.Sent = true;
                m_tokenRepository[id] = token;
            }
        }

        /// <summary>
        /// Obtém os tokens de reset de senha que foram utilizados, mas que o usuário não foi notificado do uso.
        /// </summary>
        /// <returns>Os tokens.</returns>
        public IEnumerable<UserPasswordResetToken> GetPasswordResetTokensUsedNotNotified()
        {
            return m_tokenRepository.FindAll(f => f.Used && !f.NotifiedUse);
        }

        /// <summary>
        /// Marca o token de reset de senha como uso notificado.
        /// </summary>
        /// <param name="id">O id do token.</param>
        public void MarkPasswordResetTokenAsNotifiedUse(int id)
        {
            var token = m_tokenRepository.FindBy(id);

            if (token != null)
            {
                token.NotifiedUse = true;
                m_tokenRepository[id] = token;
            }
        }

        /// <summary>
        /// Criptografa a senha do usuário.
        /// </summary>
        /// <param name="password">A senha não criptografada.</param>
        /// <returns>A senha criptografada.</returns>
        public string Encrypt(string password)
        {
            var encode = new ASCIIEncoding();
            byte[] bytPassword = encode.GetBytes(string.Concat("g&N0", password, "kbZOd%b4UO8QDk2@bKCCHHSI!Xya8Hu7"));

            var sha = new SHA512Managed();
            byte[] hash = sha.ComputeHash(bytPassword);

            var builder = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2", CultureInfo.InvariantCulture));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Verifica se a senha do usuário informado expirou.
        /// </summary>
        /// <param name="userId">O id do usuário.</param>
        /// <returns>True se a senha expirou.</returns>
        public bool IsPasswordExpired(int userId)
        {
            var lastPassword = GetLatestPasswordHistories(userId, 1).FirstOrDefault();

            return lastPassword != null && DateTime.UtcNow > lastPassword.Created.Date.AddDays(PasswordExpirationDays);
        }

        /// <summary>
        /// Obtém os últimos históricos de senha do usuário.
        /// </summary>
        /// <remarks>Os históricos são ordenados de forma decrescente pela data de criação, ou seja, as mais atuais primeiro.</remarks>
        /// <param name="userId">O id do usuário.</param>
        /// <param name="limit">O limite de resultados.</param>
        /// <returns>As senhas.</returns>
        public IEnumerable<UserPasswordHistory> GetLatestPasswordHistories(int userId, int limit)
        {
            return m_historyRepository.FindAllDescending(0, limit, f => f.UserId == userId, o => o.Created.Date);
        }
        #endregion

        #region Private methods
        private void ChangePassword(User user, string newPassword)
        {
            if (!user.Enabled)
            {
                throw new InvalidOperationException(Texts.UserDisabledCannotChangePassword);
            }

            var encryptedPassword = Encrypt(newPassword);
            SpecService.Assert(newPassword, new PasswordMustBeStrongSpec(user.Id, this));
            user.Password = encryptedPassword;
            m_userService.Save(user);
            m_historyRepository.Add(new UserPasswordHistory(user));
        }
        #endregion
    }
}
