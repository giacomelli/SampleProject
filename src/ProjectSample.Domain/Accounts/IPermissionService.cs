using ProjectSample.Infrastructure.Framework.Domain;

namespace ProjectSample.Domain.Accounts
{
    /// <summary>
    /// Define a interface para um serviço de permissões.
    /// </summary>
    public interface IPermissionService : IDomainService<Permission>
    {
    }
}
