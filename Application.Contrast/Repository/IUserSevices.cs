using Application.Common.BaseModel;
using Application.DTO.Security.UserManagement;
using Application.FreamWork.OperatonResult;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contrast.Repository
{
    public interface IUserSevices
    {
        Task<GenericOperationResult<UserProfileDto>> GetProfile(int id);
        Task<OperationResult> UpdateProfile(UpdateUserDto profile);

        Task<GenericOperationResult<UserProfileComplexResult>> GetAllUsers();
        //Task<GenericOperationResult<UserProfileDto>> GetUserById(int id);

        Task<OperationResult> RemoveUser(int id);



    }
}
