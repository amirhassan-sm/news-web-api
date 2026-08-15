using Application.Contrast.Services;
using Application.DTO.NewsCategory;
using Application.FreamWork.SearchBaseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace News.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsCategoryManagementController : ControllerBase
    {
        private readonly ICategoryNewsApplication service;
        public NewsCategoryManagementController(ICategoryNewsApplication service)
        {
            this.service= service;
            
        }
        [HttpPost]
        public async Task<IActionResult> AddNewsCategory([FromBody]CategoryAddModel model)
        {
            var result = await service.AddNewsCategory(model);
            if (!result.Success)
            {
                return StatusCode((int)(result.statusCode ?? System.Net.HttpStatusCode.BadRequest), result);
                
            }
            return Ok(result);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveNewsCategory(int id)
        {
            var result = await service.RemoveNewsCategory(id);
            if (!result.Success)
            {
                return StatusCode((int)(result.statusCode ?? System.Net.HttpStatusCode.BadRequest), result);

            }
            return Ok(result);

        }
        [HttpPut]
        public async Task<IActionResult> UpdateNewsCategory([FromBody]CategoryUpdateModel model)
        {
            var result = await service.UpdateNewsCategory(model);
            if (!result.Success)
            {
                return StatusCode((int)(result.statusCode ?? System.Net.HttpStatusCode.BadRequest), result);

            }
            return Ok(result);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryNewsById(int id)
        {
            var result = await service.GetCategoryNewsById(id);
            if (!result.Success)
            {
                return StatusCode((int)(result.statusCode ?? System.Net.HttpStatusCode.BadRequest), result);

            }
            return Ok(result);

        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategories([FromQuery] PageModel page)
        {
            var result = await service.GetAllCategories(page);
            if (!result.Success)
            {
                return StatusCode((int)(result.statusCode ?? System.Net.HttpStatusCode.BadRequest), result);

            }
            return Ok(result);

        }
        [HttpGet("Search")]
        public async Task<IActionResult> SearchCategory([FromQuery] CategorySearchModel sm)
        {
            var result = await service.SearchCategory(sm);
       
            return Ok(result);

        }

    }
}
