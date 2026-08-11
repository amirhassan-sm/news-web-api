using Application.DTO.Security.RoleManagement;
using Application.FreamWork.OperatonResult;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contrast.Repository
{
    public interface IUserRoleServices
    {
        Task<OperationResult> AssignRoleToUser(UserRoleDto model);
        Task<OperationResult> removeRoleFromUser(UserRoleDto model);
        
    }
}
