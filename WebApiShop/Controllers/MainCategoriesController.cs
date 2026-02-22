using DTO;
using System;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Reflection.Metadata.Ecma335;

namespace WebApiShop.Controllers
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
        async public Task<ActionResult<MainCategoryDTO>> AddMainCategoryAsync([FromBody] AdminMainCategoryDTO dto)
        {
            var all = await _mainCategoryService.GetMainCategoryAsync();
            if (all.Any(m => string.Equals(m.MainCategoryName, dto.MainCategoryName, StringComparison.OrdinalIgnoreCase)))
            {
                return Conflict(new { message = "Main category with the same name already exists." });
            }

            MainCategoryDTO mainCategory = await _mainCategoryService.AddMainCategoryAsync(dto);
            return CreatedAtAction(nameof(GetMainCategoryAsync), new { id = mainCategory.MainCategoryId }, mainCategory);
        }

        // PUT api/<MainCategoriesController>/5
        [HttpPut("{id}")]
        async public Task<ActionResult> Put(int id, [FromBody] AdminMainCategoryDTO dto)
        {
            if (id <= 0 || dto == null)
                return BadRequest();

            var success = await _mainCategoryService.UpdateMainCategoryAsync(id, dto);
            if (!success)
                return NotFound();

            return NoContent();
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
