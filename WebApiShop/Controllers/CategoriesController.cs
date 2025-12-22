//using Microsoft.AspNetCore.Mvc;
//using System.Text.Json;
//using Entities;
//using Services;

//namespace WebApiShope.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CategoriesController : ControllerBase
//    {
//        private readonly ICategoryService _categoryService;

//        public CategoriesController(ICategoryService categoryService)
//        {
//            _categoryService = categoryService;
//        }

//        // GET: api/<CategoriesController>
//        [HttpGet]
//        public async Task<IEnumerable<MainCategory>> GetAllAsync()
//        {
//            return await _categoryService.GetAllAsync();
//        }
//    }
//}
