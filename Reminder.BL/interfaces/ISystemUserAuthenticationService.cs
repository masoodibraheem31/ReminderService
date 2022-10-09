using Microsoft.AspNetCore.Identity;
using Reminder.BL.Helpers;
using Reminder.Entities.DTO;
using System.Threading.Tasks;

namespace Reminder.BL.interfaces
{
    public interface ISystemUserAuthenticationService
    {
        Task<Response<IdentityResult>> RegisterSystemUser(UserProfileDTO systemUser);
        Task<Response<string>> CreateRole(string RoleName);
    }
}
