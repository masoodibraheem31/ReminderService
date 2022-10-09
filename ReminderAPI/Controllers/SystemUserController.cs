using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reminder.BL.interfaces;
using Reminder.Entities.DTO;
using System.Threading.Tasks;

namespace ReminderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemUserController : ControllerBase
    {
        public readonly ISystemUserAuthenticationService service;

        public SystemUserController(ISystemUserAuthenticationService service)
        {
            this.service = service;
        }

        [HttpPost, Route("/api/v1/system-register")]
        public async Task<IActionResult> RegisterSystemUser(UserProfileDTO user)
        {
            var response = await this.service.RegisterSystemUser(user);
            return Ok(response);
        }

    }
}
