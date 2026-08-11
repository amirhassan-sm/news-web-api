using Application.Contrast.Repository;
using Application.DTO.Security.UserManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using News.Security;
using System.Net;

namespace News.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Authentication : ControllerBase
    {
        private readonly IAuthenticationService service;
        public Authentication(IAuthenticationService service)
        {
            this.service = service;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpDto model)
        {

            var result = await service.SignUp(model, AppRoles.User);
            if (!result.Success)
            {
                
                return StatusCode((int)(result.statusCode ?? HttpStatusCode.BadRequest), result);

            }
            return Ok(result);

        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var result = await service.Login(model);
            if (!result.Success)
            {

                return StatusCode((int)(result.statusCode ?? HttpStatusCode.BadRequest), result);

            }
            return Ok(result);

        }
        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh(RereshTokenDto model)
        {
            var result = await service.Refresh(model);
            if (!result.Success)
            {

                return StatusCode((int)(result.statusCode ?? HttpStatusCode.BadRequest), result);

            }
            return Ok(result);

        }



    }
}
