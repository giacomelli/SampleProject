using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using SampleProject.Infrastructure.Framework.Domain;
using SampleProject.Infrastructure.Framework.Linq;
using SampleProject.Infrastructure.Framework.Runtime;

namespace SampleProject.Domain.Accounts
{
    /// <summary>
    /// Define a interface para um serviço de domínio para usuários.
    /// </summary>
    public interface IUserService : IDomainService<User>
    {
        /// <summary>
        /// Obtém os usuários que atendem ao filtro especificado.
        /// </summary>
        /// <param name="offset">O início do índice de resultado.</param>
        /// <param name="limit">A quantidade de usuários no resultado.</param>        
        /// <param name="filter">O filtro.</param>
        /// <returns>Os usuários.</returns>
        FilterResult<User> GetByFilter(int offset, int limit, UserFilter filter);

        /// <summary>
        /// Obtém os usuários que possuem o filtro de nome completo.
        /// </summary>
        /// <param name="offset">O início do índice de resultado.</param>
        /// <param name="limit">A quantidade de usuários no resultado.</param>        
        /// <param name="filter">O filtro.</param>
        /// <returns>Os usuários.</returns>
        FilterResult<User> GetByFullName(int offset, int limit, string filter);

        /// <summary>
        /// Conta os usuários que atendem ao filtro informado.
        /// </summary>
        /// <param name="filter">O filtro.</param>
        /// <returns>O total de usuários que atendem ao filtro.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        long Count(Expression<Func<User, bool>> filter);

        /// <summary>
        /// Obtém o usuário pelo nome do usuário.
        /// </summary>
        /// <param name="userName">O nome de usuário.</param>
        /// <returns>O usuário.</returns>
        User GetByUserName(string userName);

        /// <summary>
        /// Obtém o usuário pelo e-mail.
        /// </summary>
        /// <param name="email">O e-mail do usuário.</param>
        /// <returns>O usuário.</returns>
        User GetByEmail(string email);
    
        /// <summary>
        /// Obtém o usuário pelo id do usuário externo.
        /// </summary>
        /// <param name="externalUserId">O id do usuário externo.</param>
        /// <returns>O usuário, se existir.</returns>
        User GetByExternalUserId(string externalUserId);

        /// <summary>
        /// Verifica se uma notificação pode ser enviada para o e-mail informado.
        /// </summary>
        /// <param name="email">O e-mail de destino..</param>
        /// <returns>True se pode enviar a notificação.</returns>
        bool CanSendEmailNotificationTo(string email);
    }
}
