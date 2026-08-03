using Application.Common.BaseModel;
using Application.Contrast.Repository;
using Application.DTO.Security.UserManagement;
using Application.FreamWork.OperatonResult;
using Infrastructure.Security.Idetity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Infrastructure.Security.Idetity.Services
{
    public class UserServices : IUserSevices
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly SecurityContext db;
        private readonly ILogger<UserServices> logger;
        public UserServices(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager
            , SecurityContext db, ILogger<UserServices> logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.db= db;
            this.logger = logger;
            
        }
        public async Task<GenericOperationResult<UserProfileComplexResult>> GetAllUsers()
        {
            try
            {
                var results = new UserProfileComplexResult();
                var lists = from users in db.Users
                            join userRoles in db.UserRoles on
                            users.Id equals userRoles.UserId into userRolesGroupe
                            from userRole in userRolesGroupe.DefaultIfEmpty()

                            join roles in db.Roles on

                            userRole.RoleId equals roles.Id into rolesGroupe
                            from roles in rolesGroupe.DefaultIfEmpty()
                            select new  { 
                            FirstName = users.FirstName, LastName = users.LastName,
                            UserId = users.Id,
                            UserName=users.UserName,
                            roleName =roles.Name,
                            
                            
                            
                            };

                var listIteam = lists.GroupBy(x => x.UserId).Select(x => new UserProfileListIteam
                {
                    UserId = x.First().UserId,
                    FirstName = x.First().FirstName,
                    LastName = x.Last().LastName,
                    UserName = x.First().UserName,
                    Roles = x.Select(x => x.roleName).ToList()

                });
               listIteam =  listIteam.OrderByDescending(x => x.UserId).Skip(results.pageIndex-1*results.pageSize).Take(results.pageSize);

                results.userProfiles = await listIteam.ToListAsync();

               return GenericOperationResult<UserProfileComplexResult>.ToSuccess("user list", results);



               





                            
          
                               
                
               

                
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "failed get all ");
                return GenericOperationResult<UserProfileComplexResult>.ToFail("failed to get",
                    new List<string> { "an unexpected error happens " },"Exception_Occured",HttpStatusCode.InternalServerError);

            }
        }

        public async Task<GenericOperationResult<UserProfileDto>> GetProfile(int id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id.ToString());
                if (user==null)
                {
                    return GenericOperationResult<UserProfileDto>.ToFail("failed to get", new List<string> { "this user does not exist" }

                    , "User_not_Exist", HttpStatusCode.NotFound);


                }
                var roles = await userManager.GetRolesAsync(user);
                var roleList = roles.ToList();
               

                var dto = new UserProfileDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = roleList
                };


                return GenericOperationResult<UserProfileDto>.ToSuccess("get succeed",dto);

            }
            catch (Exception ex) {
                logger.LogError(ex, "failed get {id} ",id);
                return GenericOperationResult<UserProfileDto>.ToFail("failed to get",
                    new List<string> { "an unexpected error happens " }, "Exception_Occured", HttpStatusCode.InternalServerError);



            }
        }


        public async Task<OperationResult> RemoveUser(int id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id.ToString());
                if (user ==null)
                {
                    return OperationResult.ToFail(id,"failed to remove",new List<string> {"this user does not exist"},
                        "User_Not_Exist",HttpStatusCode.NotFound);
                    
                }

                var result = await userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return OperationResult.ToFail(id, "failed to remove", result.Errors.Select(x=>x.Description).ToList() ,
                        "Remove_Failed", HttpStatusCode.BadRequest);

                }
                return OperationResult.ToSuccess(id, "user removed successfully");



            }
            catch(Exception ex)
            {
                logger.LogError(ex, "failed to remove user{id}", id);
                return OperationResult.ToFail(id, "failed to remove", new List<string> { "an unexpected error occured" }, "" +
                    "Exception_Occured", HttpStatusCode.InternalServerError);

            }
        }

        public async Task<OperationResult> UpdateProfile(UpdateUserDto profile)
        {
            try
            {
                var user = await userManager.FindByIdAsync(profile.UserId);
                if (user == null)
                {
                    return OperationResult.ToFail("failed to update", new List<string> { "this user does not exist" },
                        "User_Not_Exist", HttpStatusCode.NotFound);

                }
                user.FirstName = profile.FirstName;
                user.LastName = profile.LastName;
               
                
                if (!string.IsNullOrEmpty(profile.NewPassword))
                {
                    var resultPass = await userManager.ChangePasswordAsync(user, profile.CurrentPassword, profile.NewPassword);
                    if (!resultPass.Succeeded)
                    {
                        return OperationResult.ToFail("failed to update password", resultPass.Errors.Select(x => x.Description).ToList(),
                            "Failed_ToUpdate_Password", HttpStatusCode.BadRequest);
                    }


                }
                var userNameResult = await userManager.SetUserNameAsync(user, profile.UserName);

                if (!userNameResult.Succeeded)
                {
                    return OperationResult.ToFail(
                        "failed to update username",
                        userNameResult.Errors.Select(x => x.Description).ToList(),
                        "Failed_ToUpdate_Username",
                        HttpStatusCode.BadRequest
                    );
                }
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return OperationResult.ToSuccess("Update succeed");

                }
                return OperationResult.ToFail("failed to update ", result.Errors.Select(x => x.Description).ToList(),
                            "Failed_ToUpdate", HttpStatusCode.BadRequest);

            }
            catch (Exception ex) {
                logger.LogError(ex, "Failed to update user {userId}", profile.UserId);
                return OperationResult.ToFail("failed to update ",new List<string> { "an unexpected error occured"},
                           "Failed_ToUpdate", HttpStatusCode.InternalServerError);


            }
        }
    }
}
