using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Reminder.BL.Helpers;
using Reminder.BL.interfaces;
using Reminder.Entities;
using Reminder.Entities.DTO;
using Reminder.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace Reminder.BL.implementations
{
    public class SystemUserAuthenticationService : ISystemUserAuthenticationService
    {
        private readonly UserManager<SystemUser> _userManager;
        private readonly SignInManager<SystemUser> signInManager;
        private readonly RoleManager<SystemRole> roleManager;

        private const string UserRegisteredSuccessfully = "System User Registered Successfully";
        private const string UserRegistrationFailed = "System User Registration Failed";
        private const string RoleCreatedSucess = "Role created successfully";
        private const string RoleCreateFailed = "Role creation failed";
        private const string RoleExists = "Role already exists";
        public SystemUserAuthenticationService(UserManager<SystemUser> userManager, SignInManager<SystemUser> signInManager, RoleManager<SystemRole> roleManager)
        {
            _userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        public async Task<Response<string>> CreateRole(string RoleName)
        {
            string message = string.Empty;
            var roleInstance = await roleManager.RoleExistsAsync(RoleName);
            if (!roleInstance)
            {
                var role = new SystemRole();
                role.Name = RoleName;
                var roleResult = await roleManager.CreateAsync(role);
                if (roleResult.Succeeded)
                    message = RoleCreatedSucess;
                else
                    message = RoleCreateFailed;
            }
            else
                message = RoleExists;

            return new Response<string>
            {
                Message = message,
                Success = true,
            };
        }
        public async Task<Response<IdentityResult>> RegisterSystemUser(UserProfileDTO user)
        {
            var roleInstance = await roleManager.RoleExistsAsync("user");
            if (!roleInstance)
            {
                var role = new SystemRole();
                role.Name = "user";
                await roleManager.CreateAsync(role);   
            }
            SystemUser _user = new SystemUser()
            {
                Email = user.Email,
                UserName = user.Username,
                Name = user.Name
            };
            var result = await _userManager.CreateAsync(_user, user.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, "user");
            }
            return new Response<IdentityResult>()
            {
                Success = true,
                Data = result,
                Message = result.Succeeded ? UserRegisteredSuccessfully : UserRegistrationFailed
            };
        }
      
    }
}
