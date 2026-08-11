using Application.Common.BaseModel;
using Application.DTO.Security.UserManagement;
using Application.FreamWork.OperatonResult;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contrast.Repository
{
    public interface IAuthenticationService
    {
        Task<GenericOperationResult<TokenResult>> Login(LoginDto dto);

        Task<OperationResult> SignUp(SignUpDto dto, string UserRole);

        Task<GenericOperationResult<TokenResult>> Refresh(RereshTokenDto dto);
    }
}
