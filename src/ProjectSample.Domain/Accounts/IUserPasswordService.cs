using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ProjectSample.Domain.Accounts
{
    /// <summary>
    /// Define a interface para o serviço de domínio para o reset de senha do usuário.
    /// </summary>
    public interface IUserPasswordService
    {
        /// <summary>
        /// Reseta a senha do usuário.
        /// </summary>
        /// <param name="tokenCode">O código do token que será utilizado para resetar a senha.</param>
        /// <param name="newPassword">A nova senha.</param>
        /// <returns>O usuário com a senha resetada.</returns>
        User ResetPassword(Guid tokenCode, string newPassword);

        /// <summary>
        /// Altera a senha do usuário.
        /// </summary>
        /// <param name="userId">O id do usuário.</param>
        /// <param name="oldPassword">A senha antiga.</param>
        /// <param name="newPassword">A nova senha.</param>
        void ChangePassword(int userId, string oldPassword, string newPassword);

        /// <summary>
        /// Realiza a geração de um token para redefinição de senha do usuário com o e-mail informado.
        /// </summary>
        /// <param name="email">O e-mail do usuário.</param>
        /// <returns>O token gerado.</returns>
        UserPasswordResetToken GeneratePasswordResetToken(string email);

        /// <summary>
        /// Obtém os tokens de reset de senha que ainda não foram enviados aos usuários.
        /// </summary>
        /// <returns>Os tokens não enviados.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        IEnumerable<UserPasswordResetToken> GetPasswordResetTokensNotSent();

        /// <summary>
        /// Obtém os tokens de reset de senha que foram utilizados, mas que o usuário não foi notificado do uso.
        /// </summary>
        /// <returns>Os tokens.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        IEnumerable<UserPasswordResetToken> GetPasswordResetTokensUsedNotNotified();

        /// <summary>
        /// Marca o token de reset de senha como enviado.
        /// </summary>
        /// <param name="id">O id do token.</param>
        void MarkPasswordResetTokenAsSent(int id);

        /// <summary>
        /// Marca o token de reset de senha como uso notificado.
        /// </summary>
        /// <param name="id">O id do token.</param>
        void MarkPasswordResetTokenAsNotifiedUse(int id);

        /// <summary>
        /// Criptografa a senha do usuário.
        /// </summary>
        /// <param name="password">A senha não criptografada.</param>
        /// <returns>A senha criptografada.</returns>
        string Encrypt(string password);

        /// <summary>
        /// Verifica se a senha do usuário informado expirou.
        /// </summary>
        /// <param name="userId">O id do usuário.</param>
        /// <returns>True se a senha expirou.</returns>
        bool IsPasswordExpired(int userId);

        /// <summary>
        /// Obtém os últimos históricos de senha do usuário.
        /// </summary>
        /// <remarks>Os históricos são ordenados de forma decrescente pela data de criação, ou seja, as mais atuais primeiro.</remarks>
        /// <param name="userId">O id do usuário.</param>
        /// <param name="limit">O limite de resultados.</param>
        /// <returns>As senhas.</returns>
        IEnumerable<UserPasswordHistory> GetLatestPasswordHistories(int userId, int limit);
    }
}
