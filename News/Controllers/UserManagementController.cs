using Application.Contrast.Repository;
using Application.DTO.Security.UserManagement;
using Infrastructure.Security.Idetity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using News.Security;
using System.ClientModel.Primitives;
using System.Net;
using System.Security.Claims;

namespace News.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserSevices service;
        public UserManagementController(IUserSevices service)
        {
            this.service = service;
        }
        [HttpGet("getProfile/{id}")]
        public async Task<IActionResult> GetProfile(int id)
        {
            var result = await service.GetProfile(id);
            if (!result.Success)
            {
                return StatusCode((int)(result.statusCode ?? HttpStatusCode.BadRequest), result);

            }
            return Ok(result);


        }
        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await service.GetAllUsers();
            if (!result.Success)
            {
                return StatusCode((int)(result.statusCode ?? HttpStatusCode.BadRequest), result);

            }
            return Ok(result);


        }
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDto model)
        {
            var result = await service.UpdateProfile(model);
            if (!result.Success)
            {
                return StatusCode((int)(result.statusCode ?? HttpStatusCode.BadRequest), result);

            }
            return Ok(result);


        }
        [HttpDelete("RemoveUser")]
        public async Task<IActionResult> RemoveUser(int id)
        {
            var result = await service.RemoveUser(id);
            if (!result.Success)
            {
                return StatusCode((int)(result.statusCode ?? HttpStatusCode.BadRequest), result);

            }
            return Ok(result);


        }

        [HttpPost("AddMyImageProfile")]
        [Authorize(Roles = $"{AppRoles.User},{AppRoles.Admin}")]
        public async Task<IActionResult> AddMyImageProfile(UserImageDtoEntry model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out var id))
            {
                return Unauthorized(
                   Application.FreamWork.OperatonResult.OperationResult.ToFail(
                        "Failed to add image",
                        new List<string> { "Invalid user identity" }
                    )
                );
            }

            var dto = new UserImageDto
            {
                ProfileImage = model.ProfileImage,
                UserId = id
            };

            var result = await service.AddUserImageProfile(dto);

            if (!result.Success)
            {
                return StatusCode(
                    (int)(result.statusCode ?? HttpStatusCode.BadRequest),
                    result
                );
            }

            return Ok(result);
        }

    }
}
