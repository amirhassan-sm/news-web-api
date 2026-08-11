using Application.Contrast.Repository;
using Application.DTO.Security.RoleManagement;
using Application.FreamWork.OperatonResult;
using Infrastructure.Security.Idetity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Infrastructure.Security.Identity.Services
{
    public class UserRoleServices : IUserRoleServices   
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ILogger<UserRoleServices> logger;
        public UserRoleServices(UserManager<ApplicationUser> userManager 
            , RoleManager<ApplicationRole> roleManager,
            ILogger<UserRoleServices> logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
            
        }
        public async Task<OperationResult> AssignRoleToUser(UserRoleDto model)
        {
            try
            {
                var user = await userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                {
                    return OperationResult.ToFail("failed to assign", new List<string> { "this user does not exist" }
                    ,"User_Not_Exist",HttpStatusCode.NotFound);


                }
                var role = await roleManager.FindByIdAsync(model.RoleId.ToString());
                if (role == null)
                {

                    return OperationResult.ToFail("failed to assign", new List<string> { "this role does not exist" },
                        "Role_Not_Found",HttpStatusCode.NotFound);

                }
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    return OperationResult.ToFail("failed to assign role", new List<string> { "user  already has this role" },
                        "role_already_Assigned",HttpStatusCode.BadRequest);

                }
                var result = await userManager.AddToRoleAsync(user, role.Name);
                if (!result.Succeeded)
                {
                    return OperationResult.ToFail("add failed ", result.Errors.Select(x => x.Description).ToList(),"Update_Failed",
                        HttpStatusCode.BadRequest);


                }
                return OperationResult.ToSuccess(user.Id, $"the role: {role.Name} successfully assigned to user: {user.UserName}");
            }
            catch (Exception ex)
            {
                logger.LogError(
    ex,
    "Assign role failed. UserId: {UserId}, RoleId: {RoleId}",
    model.UserId,
    model.RoleId);
                return OperationResult.ToFail("failed to assign role", new List<string> { "an uexpected error occured " }
                ,"Exception_Occured",HttpStatusCode.InternalServerError);

            }
        }

        public async Task<OperationResult> removeRoleFromUser(UserRoleDto model)
        {
            try
            {

                var user = await userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                {
                    return OperationResult.ToFail("failed to remove", new List<string> { "this user does not exist" }
                        , "User_Not_Exist", HttpStatusCode.NotFound);


                }
                var role = await roleManager.FindByIdAsync(model.RoleId.ToString());
                if (role == null)
                {

                    return OperationResult.ToFail("failed to remove", new List<string> { "this role does not exist" },
                         "Role_Not_Found", HttpStatusCode.NotFound);

                }
                if (!await userManager.IsInRoleAsync(user, role.Name))
                {
                    return OperationResult.ToFail("failed to remove role from this user",
                        new List<string> { $"the user{user.UserName} does not assigned to the role {role.Name}" },
                           "role_already_Assigned", HttpStatusCode.BadRequest);

                }
                var result = await userManager.RemoveFromRoleAsync(user, role.Name);

                if (!result.Succeeded)
                {
                    return OperationResult.ToFail("failed to remove this role", result.Errors.Select(x => x.Description).ToList());

                }
                return OperationResult.ToSuccess(user.Id, $"the role : {role.Name} removed successfully from the user:{user.UserName}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to remove role from user", model.UserId, model.RoleId);
                return OperationResult.ToFail("Failed to remove role", new List<string> { "an uexpected error occured " }
                , "Exception_Occured", HttpStatusCode.InternalServerError);

            }
        }
    }
}
