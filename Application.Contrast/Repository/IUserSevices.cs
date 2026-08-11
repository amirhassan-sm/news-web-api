using Application.Common.BaseModel;
using Application.DTO.Security.UserManagement;
using Application.FreamWork.OperatonResult;

namespace Application.Contrast.Repository
{
    public interface IUserSevices
    {
        Task<GenericOperationResult<UserProfileDto>> GetProfile(int id);
        Task<OperationResult> UpdateProfile(UpdateUserDto profile);

        Task<GenericOperationResult<UserProfileComplexResult>> GetAllUsers();

    

        Task<OperationResult> RemoveUser(int id);
        Task<OperationResult> AddUserImageProfile(UserImageDto model);



    }
}
