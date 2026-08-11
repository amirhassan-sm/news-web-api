using Application.Common.BaseModel;
using Application.Contrast.Repository;
using Application.DTO.Security.RoleManagement;
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
    
    
    public class RoleServices : IRoleService
    {
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ILogger<RoleServices> logger; 
        private readonly SecurityContext db;
        public RoleServices(RoleManager<ApplicationRole> roleManager, ILogger<RoleServices> logger, SecurityContext db)
        {
            this.roleManager = roleManager;
            this.logger = logger;
            this.db = db;
        }
        public async Task<OperationResult> AddRoleAsync(AddRoleDto model)
        {
            try
            {
                var role = new ApplicationRole
                {
                    Name = model.RoleName,


                };
                if (await roleManager.RoleExistsAsync(model.RoleName))
                {
                    return OperationResult.ToFail("failed to add role", new List<string> { "this role already exist" }, "Role_Already_Exist",
                        HttpStatusCode.BadRequest);
                    
                }
                var result = await roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    return OperationResult.ToFail("failed to add role", result.Errors.Select(x => x.Description).ToList(), "AddRole_Failed",
                        HttpStatusCode.BadRequest);
                }
                return OperationResult.ToSuccess("role added successfully");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "failed to add role {roleName}", model.RoleName);
               
                return OperationResult.ToFail("failed to add role", new List<string> {"an unexpected error occured"}, "Exception_Occured",
                      HttpStatusCode.InternalServerError);
            }
        }

        public async Task<OperationResult> DeleteRoleAsync(int id)
        {
            try
            {
                var role = await roleManager.FindByIdAsync(id.ToString());
                if (role == null)
                {
                    return OperationResult.ToFail("failed to delete role", new List<string> { "this role does not exist" }, "Role_Not_Exist",
                        HttpStatusCode.BadRequest);
                    
                }
                var result = await roleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    return OperationResult.ToFail("failed to delete role", result.Errors.Select(x=>x.Description).ToList(), "Failed_Remove_Role",
                      HttpStatusCode.BadRequest);

                }

                return OperationResult.ToSuccess(id,"Role removed successfully");

            }
            catch (Exception ex) {
                logger.LogError(ex, "failed to remove role {id}", id);

                return OperationResult.ToFail("failed to remove role", new List<string> { "an unexpected error occured" }, "Exception_Occured",
                      HttpStatusCode.InternalServerError);

            }
        }

        public async Task<OperationResult> EditRoleAsyn(EditRoleDto model)
        {
            try {
                var role = await roleManager.FindByIdAsync(model.RoleId.ToString());
                if (role == null)
                {
                    return OperationResult.ToFail("failed to update role", new List<string> { "this role does not exist" }, "Role_Not_Exist",
                      HttpStatusCode.BadRequest);


                }
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);
                if (!result.Succeeded)
                {
                    return OperationResult.ToFail("failed to update role", result.Errors.Select(x => x.Description).ToList(), "Failed_Update_Role",
                      HttpStatusCode.BadRequest);

                }
                return OperationResult.ToSuccess(role.Id, "Role updated succesfully");



            }
            catch (Exception ex) {
                logger.LogError(ex, "Failed to update role {id}", model.RoleId);
                return OperationResult.ToFail("failed to update role", new List<string> { "an unexpected error occured" }, "Exception_Occured",
                      HttpStatusCode.InternalServerError);

            }
        }

        public async Task<GenericOperationResult<List<EditRoleDto>>> GetAllRolesAsync()
        {
            try
            {
                var roles = from item in db.Roles select new EditRoleDto
                {
                    RoleId = item.Id,
                    RoleName = item.Name ?? ""
                };
                var list= await roles.AsNoTracking().ToListAsync();
                return GenericOperationResult<List<EditRoleDto>>.ToSuccess("role lists",list);

                 
                
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Failed to get all roles");
                return GenericOperationResult<List<EditRoleDto>>.ToFail("failed to get roles"
                    , new List<string> { "an unexpected error occured" }, "Exception_Occured",
                      HttpStatusCode.InternalServerError);

            }
        }

        public async Task<GenericOperationResult<EditRoleDto>> GetRolesByIdAsync(int id)
        {
            try
            {
                var role = await roleManager.FindByIdAsync(id.ToString());
                if (role == null)
                {
                    return GenericOperationResult<EditRoleDto>.ToFail("failed to get role", 
                        new List<string> { "this role does not exist" }, "Role_Not_Exist",
              HttpStatusCode.BadRequest);


                }
                var model = new EditRoleDto { RoleId = role.Id,RoleName = role.Name ??"" };
                return GenericOperationResult<EditRoleDto>.ToSuccess("get succeed", model);


            }
            catch (Exception ex) {
                logger.LogError(ex, "Failed to update role {id}", id);
                return GenericOperationResult<EditRoleDto>.ToFail("failed to update role", new List<string> { "an unexpected error occured" }, "Exception_Occured",
                      HttpStatusCode.InternalServerError);
            }
        }
    }
}
