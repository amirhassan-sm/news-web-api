using Application.Contrast.Repository;
using Application.DTO.Security.RoleManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace News.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleManagement : ControllerBase
    {
        private readonly IUserRoleServices services;
        public UserRoleManagement(IUserRoleServices services)
        {
            this.services = services;
            
        }
        [HttpPost("AssignRoleToUser")]
        public async Task<IActionResult> AssignRoleToUser(UserRoleDto model)
        {
            var result = await services.AssignRoleToUser(model);
            if (!result.Success)
            {
                return StatusCode((int)(result.statusCode ??  System.Net.HttpStatusCode.BadRequest),result);
                
            }
            return Ok(result);
        }
        [HttpPost("RemoveRoleFromUser")]
        public async Task<IActionResult> RemoveRoleFromUser(UserRoleDto model)
        {
            var result = await services.removeRoleFromUser(model);
            if (!result.Success)
            {
                return StatusCode((int)(result.statusCode ?? System.Net.HttpStatusCode.BadRequest),result);

            }
            return Ok(result);
        }

    }
}
