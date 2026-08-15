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

namespace Infrastructure.Security.Identity.Services
{
    public class UserServices : IUserSevices
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly SecurityContext db;
        private readonly ILogger<UserServices> logger;
        private readonly IUserProfileStorage profileStorage;
        public UserServices(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager
            , SecurityContext db, ILogger<UserServices> logger, IUserProfileStorage profileStorage)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.db = db;
            this.logger = logger;
            this.profileStorage = profileStorage;

        }


        private async Task<OperationResult> ValidateImage(UserImageDto model)
        {
            if (model.ProfileImage == null)
            {
                return OperationResult.ToFail("Inavlid Image File", new List<string> { "image is requierd" }, "Image_Not_Exist"
                    , HttpStatusCode.BadRequest);

            }
            const long maxFileSize = 10 * 1024 * 1024;//10mb
            if (model.ProfileImage.Length <= 0)
            {
                return OperationResult.ToFail(
                    "Upload failed",
                    new List<string> { "File is empty." },
                    "Empty_File", HttpStatusCode.BadRequest);
            }

            if (model.ProfileImage.Length > maxFileSize)
            {
                return OperationResult.ToFail(
                    "Upload failed",
                    new List<string> { "Maximum allowed size is 5 MB." },
                    "File_Too_Large", HttpStatusCode.BadRequest);
            }
            var extension = Path.GetExtension(model.ProfileImage?.FileName)?.ToLowerInvariant();

            var allowedExtensions = new[]
  {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp",

    };

            if (!allowedExtensions.Contains(extension))
            {
                return OperationResult.ToFail(
                    "Upload failed",
                    new List<string> { "Only JPG, JPEG, PNG and WEBP images are allowed." },
                    "Invalid_File_Extension",HttpStatusCode.BadRequest);
            }

            return OperationResult.ToSuccess("image is valid");



        }

        public async Task<OperationResult> AddUserImageProfile(UserImageDto model)
        {
            try
            {
                var validate = await ValidateImage(model);
                if (!validate.Success)
                {
                    return validate;
                }
                var user = await userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                {
                    return OperationResult.ToFail("failed to add image", new List<string> { "this image " },"User_Not_Exist"
                        ,HttpStatusCode.NotFound);
                    
                }
                if (user.ProfileImageUrl !=null)
                {
                    await profileStorage.DeleteProfileAsync(user.ProfileImageUrl);  
                    
                }

                var url = await profileStorage.SaveProfileAsync(model.ProfileImage.OpenReadStream(), model.ProfileImage.FileName);
                
                user.ProfileImageUrl = url;

                var result = await userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                  await  profileStorage.DeleteProfileAsync(url);
                    return OperationResult.ToFail("failed to add image", result.Errors.Select(x => x.Description).ToList()
                        , "Failed_To_Update", HttpStatusCode.BadRequest);
                    
                }
                return OperationResult.ToSuccess("Image added successfully");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "failed to add image to user {userId}", model.UserId);
                return OperationResult.ToFail("failed to add image ", new List<string> { "an inexpected error occured" },
                    "Exception_Occured", HttpStatusCode.InternalServerError);

            }
        }

        public async Task<GenericOperationResult<UserProfileComplexResult>> GetAllUsers()
        {
            try
            {
                var results = new UserProfileComplexResult();

                var lists =
                    from users in db.Users
                    where !users.IsDeleted

                    join userRoles in db.UserRoles
                        on users.Id equals userRoles.UserId into userRolesGroup

                    from userRole in userRolesGroup.DefaultIfEmpty()

                    join roles in db.Roles
                        on userRole.RoleId equals roles.Id into rolesGroup

                    from role in rolesGroup.DefaultIfEmpty()

                    select new
                    {
                        FirstName = users.FirstName,
                        LastName = users.LastName,
                        UserId = users.Id,
                        UserName = users.UserName,
                        RoleName = role != null ? role.Name : null,
                        ImageUrl = users.ProfileImageUrl
                    };

                var listItem = lists
                    .GroupBy(x => x.UserId)
                    .Select(x => new UserProfileListIteam
                    {
                        UserId = x.First().UserId,
                        FirstName = x.First().FirstName,
                        LastName = x.First().LastName,
                        UserName = x.First().UserName,
                        Roles = x
                            .Where(r => r.RoleName != null)
                            .Select(r => r.RoleName)
                            .ToList(),
                        ProfileImage = x.First().ImageUrl
                    })
                    .OrderByDescending(x => x.UserId)
                    .Skip((results.pageIndex - 1) * results.pageSize)
                    .Take(results.pageSize);

                results.userProfiles = await listItem.ToListAsync();
                results.RecordCount = await listItem.CountAsync();  

                return GenericOperationResult<UserProfileComplexResult>
                    .ToSuccess("user list", results);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "failed get all");

                return GenericOperationResult<UserProfileComplexResult>.ToFail(
                    "failed to get",
                    new List<string> { "an unexpected error happens" },
                    "Exception_Occured",
                    HttpStatusCode.InternalServerError);
            }
        }

        public async Task<GenericOperationResult<UserProfileDto>> GetProfile(int id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    return GenericOperationResult<UserProfileDto>.ToFail("failed to get", new List<string> { "this user does not exist" }

                    , "User_not_Exist", HttpStatusCode.NotFound);


                }
                if (user.IsDeleted)
                {
                    return GenericOperationResult<UserProfileDto>.ToFail("failed to get", new List<string> { "this user deleted" }

                    , "User_Delted", HttpStatusCode.NotFound);

                }
                var roles = await userManager.GetRolesAsync(user);
                var roleList = roles.ToList();


                var dto = new UserProfileDto
                {
                    UserId = user.Id,
                    UserName = user.UserName??"",
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = roleList , 
                    ImageUrl = user.ProfileImageUrl ??""
                    ,Bio = user.Bio ??""
                    
                };


                return GenericOperationResult<UserProfileDto>.ToSuccess("get succeed", dto);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "failed get {id} ", id);
                return GenericOperationResult<UserProfileDto>.ToFail("failed to get",
                    new List<string> { "an unexpected error happens " }, "Exception_Occured", HttpStatusCode.InternalServerError);



            }
        }

       

        public async Task<OperationResult> RemoveUser(int id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    return OperationResult.ToFail(id, "failed to remove", new List<string> { "this user does not exist" },
                        "User_Not_Exist", HttpStatusCode.NotFound);

                }
                if(user.IsDeleted == true)
                {
                    return OperationResult.ToFail(id, "failed to remove", new List<string> { "this user already deleted" },
                    "User_Already_Deleted", HttpStatusCode.BadRequest);
                }
                user.IsDeleted = true;

                var result = await userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return OperationResult.ToFail(id, "failed to remove", result.Errors.Select(x => x.Description).ToList(),
                        "Remove_Failed", HttpStatusCode.BadRequest);

                }
                return OperationResult.ToSuccess(id, "user removed successfully");



            }
            catch (Exception ex)
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
                if (user.IsDeleted)
                {
                    return OperationResult.ToFail("failed to get", new List<string> { "this user deleted" }

                    , "User_Delted", HttpStatusCode.NotFound);

                }
                user.FirstName = profile.FirstName;
                user.LastName = profile.LastName;
                user.Bio = profile.Bio;




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
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to update user {userId}", profile.UserId);
                return OperationResult.ToFail("failed to update ", new List<string> { "an unexpected error occured" },
                           "Failed_ToUpdate", HttpStatusCode.InternalServerError);


            }
        }
    }
}
