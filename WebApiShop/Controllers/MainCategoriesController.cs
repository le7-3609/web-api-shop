using DTO;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Reflection.Metadata.Ecma335;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainCategoriesController : ControllerBase
    {
        private readonly IMainCategoryService _mainCategoryService;
        public MainCategoriesController(IMainCategoryService mainCategoryService)
        {
            _mainCategoryService = mainCategoryService;
        }

        // GET: api/<MainCategoriesController>
        [HttpGet]
        async public Task<ActionResult<IEnumerable<MainCategoryDTO>>> GetMainCategoryAsync()
        {
            IEnumerable<MainCategoryDTO> MainCategoriesList = await _mainCategoryService.GetMainCategoryAsync();
            if (MainCategoriesList == null)
            {
                return NoContent();
            }
            return Ok(MainCategoriesList);
        }


        // POST api/<MainCategoriesController>
        [HttpPost]
        async public Task<ActionResult<MainCategoryDTO>> AddMainCategoryAsync([FromBody] ManegerMainCategoryDTO dto)
        {
            MainCategoryDTO mainCategory = await _mainCategoryService.AddMainCategoryAsync(dto);
            return CreatedAtAction(nameof(GetMainCategoryAsync), new { id = mainCategory.MainCategoryId }, mainCategory); 
        }

        // PUT api/<MainCategoriesController>/5
        [HttpPut("{id}")]
        async public Task Put(int id, [FromBody] MainCategoryDTO dto)
        {
            await _mainCategoryService.UpdateMainCategoryAsync(id, dto);
        }

        // DELETE api/<MainCategoriesController>/5
        [HttpDelete("{id}")]
        async public Task<ActionResult> DeleteMainCategoryAsync(int id)
        {
            bool flag = await _mainCategoryService.DeleteMainCategoryAsync(id);
            if (flag)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
