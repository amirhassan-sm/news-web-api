using Application.Contrast.Repository;
using Application.DTO.Security.RoleManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace News.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleManagement : ControllerBase
    {
        private readonly IRoleService service;
        public RoleManagement(IRoleService service)
        {
            this.service = service;
            
        }
        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole(AddRoleDto model)
        {
            var result = await service.AddRoleAsync(model);
            if (!result.Success)
            {
                return StatusCode((int)(result.statusCode ?? HttpStatusCode.BadRequest),result);
                
            }
            return Ok(result);
        }
        [HttpPut("EditRole")]
        public async Task<IActionResult> EditRole(EditRoleDto model)
        {
            var result = await service.EditRoleAsyn(model);
            if (!result.Success)
            {
                return StatusCode((int)(result.statusCode ?? HttpStatusCode.BadRequest), result);

            }
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await service.DeleteRoleAsync(id);
            if (!result.Success)
            {
                return StatusCode((int)(result.statusCode ?? HttpStatusCode.BadRequest), result);

            }
            return Ok(result);
        }
        [HttpGet("{id}")]

        public async Task<IActionResult> GetRolesByIdAsync(int id)
        {
            var result = await service.GetRolesByIdAsync(id);
            if (!result.Success)
            {
                return StatusCode((int)(result.statusCode ?? HttpStatusCode.BadRequest), result);

            }
            return Ok(result);
        }
        [HttpGet("GetAllRoles")]

        public async Task<IActionResult> GetAllRoles()
        {
            var result = await service.GetAllRolesAsync();
            if (!result.Success)
            {
                return StatusCode((int)(result.statusCode ?? HttpStatusCode.BadRequest), result);

            }
            return Ok(result);
        }
    }
}
