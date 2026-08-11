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
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Xml.Linq;

namespace Infrastructure.Security.Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IGenerateToken generateToken;
        private readonly ILogger<AuthenticationService> logger;


        public AuthenticationService(UserManager<ApplicationUser> userManager, IGenerateToken _generateToke
            , ILogger<AuthenticationService> logger)
        {
            this.userManager = userManager;
            this.generateToken = _generateToke;
            this.logger = logger;
            

        }
        public async Task<GenericOperationResult<TokenResult>> Login(LoginDto dto)
        {
            try
            {
                var user = await userManager.FindByNameAsync(dto.userName);
                if (user == null)
                {
                    return GenericOperationResult<TokenResult>.ToFail("failed to login",
                        new List<string> { "invalid username or password" }, "Invalid_Credentials", HttpStatusCode.NotFound);


                }
                if (user.IsDeleted)
                {
                    return GenericOperationResult<TokenResult>.ToFail("failed to login",
                         new List<string> { "this user is deleted" }, "User_Deleted", HttpStatusCode.NotFound);
                }
                var ValidatePassword = await userManager.CheckPasswordAsync(user, dto.Password);
                if (!ValidatePassword)
                {
                    return GenericOperationResult<TokenResult>.ToFail("failed to login",
                      new List<string> { "invalid username or password" }, "Invalid_Credentials", HttpStatusCode.NotFound);

                }
                var Accesstoken = await generateToken.GenerateAcsessToken(user.Id, user.UserName, user.FirstName, user.LastName);

                var refreshToken = generateToken.GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiration= DateTime.UtcNow.AddDays(1);

                var saveRefreshToken = await userManager.UpdateAsync(user);

                if (!saveRefreshToken.Succeeded)
                {
                    return GenericOperationResult<TokenResult>.ToFail("failed to login",
                      saveRefreshToken.Errors.Select(x => x.Description).ToList(), "Failef_ToSave_RefreshToken"
                      , HttpStatusCode.InternalServerError);

                }
                return GenericOperationResult<TokenResult>.ToSuccess("login succeed", new TokenResult
                {
                    refreshToken = refreshToken,
                    token = Accesstoken
                });




            }
            catch (Exception ex)
            {
                logger.LogError(ex, "failed to login {userName} ", dto.userName);

                return GenericOperationResult<TokenResult>.ToFail("failed to login",
                      new List<string> {"an unexpected error occured"}, "Exception_Occured"
                      , HttpStatusCode.InternalServerError);


            }
        }

        public async Task<GenericOperationResult<TokenResult>> Refresh(RereshTokenDto dto)
        {
            try
            {
                var user = await userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == dto.RefreshToken);
                if (user == null)
                {
                    return GenericOperationResult<TokenResult>.ToFail("failed to refresh",
                      new List<string> { "invalid refreshtoken" }, "Invalid_refreshToken", HttpStatusCode.Unauthorized);

                }
                if (user.IsDeleted)
                {
                    return GenericOperationResult<TokenResult>.ToFail("failed to login",
                         new List<string> { "this user is deleted" }, "User_Deleted", HttpStatusCode.NotFound);
                }
                if (user.RefreshTokenExpiration <= DateTime.UtcNow)
                {
                    return GenericOperationResult<TokenResult>.ToFail("failed to refresh",
                     new List<string> { "Invalid refresh token." }, "Invalid_refreshToken", HttpStatusCode.NotFound);

                }
                var NewRefToken = generateToken.GenerateRefreshToken();
                user.RefreshToken = NewRefToken;
                user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(1);
                var result = await userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return GenericOperationResult<TokenResult>.ToFail("failed to refresh",
                     new List<string> { "failed to save refresh token" }, "Save_refreshToken_failed", HttpStatusCode.InternalServerError);


                }
                var token = await generateToken.GenerateAcsessToken(user.Id, user.UserName, user.FirstName, user.LastName);

                return GenericOperationResult<TokenResult>.ToSuccess("refresh succeed", new TokenResult
                {
                    refreshToken = NewRefToken,
                    token = token,

                });



            }
            catch (Exception ex) {
                logger.LogError(ex, "failed to refresh token ");

                return GenericOperationResult<TokenResult>.ToFail("failed to refresh",
                      new List<string> { "an unexpected error occured" }, "Exception_Occured"
                      , HttpStatusCode.InternalServerError);

            }
        }

        public async Task<OperationResult> SignUp(SignUpDto dto,string UserRole)
        {
            try
            {
                var user = new ApplicationUser
                {
                    UserName = dto.UserName,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                };

                var result = await userManager.CreateAsync(user, dto.PassWord);
                if (!result.Succeeded)
                {
                    return OperationResult.ToFail("failed to sign up", result.Errors.Select(x => x.Description).ToList(),"signUp_failed"
                        , HttpStatusCode.BadRequest);
                    
                }
                await userManager.AddToRoleAsync(user,UserRole);

                return OperationResult.ToSuccess("sign up suceed");


            }
            catch (Exception ex) {
                logger.LogError(ex, "failed to sign up {userName}", dto.UserName);
             

                return OperationResult.ToFail("failed to sign up", new List<string> {" an unexpected error occured"},"Exception_Occured",
                    HttpStatusCode.InternalServerError);
            
            }
        }
    }
}
