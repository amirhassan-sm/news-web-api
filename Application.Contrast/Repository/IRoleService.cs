using Application.Common.BaseModel;
using Application.DTO.Security.RoleManagement;
using Application.FreamWork.OperatonResult;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contrast.Repository
{
    public interface IRoleService
    {
         Task<OperationResult> AddRoleAsync(AddRoleDto model);
         Task<OperationResult> EditRoleAsyn(EditRoleDto model);

         Task<OperationResult> DeleteRoleAsync(int id);
         Task<GenericOperationResult<EditRoleDto>> GetRolesByIdAsync(int id);
         Task<GenericOperationResult<List<EditRoleDto>>> GetAllRolesAsync();

    }
}
