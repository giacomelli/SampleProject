using SampleProject.Infrastructure.Framework.Domain;

namespace SampleProject.Domain.Accounts
{
    /// <summary>
    /// Define a interface para um serviço de permissões.
    /// </summary>
    public interface IPermissionService : IDomainService<Permission>
    {
    }
}
