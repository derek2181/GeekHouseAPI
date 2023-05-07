using GeeekHouseAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Controllers
{
    [ApiController]
    [Route("categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> listAllCategories()
        {
            try
            {
                var categories = await _categoryRepository.getAllCategories();
                return Ok(categories);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("subcategories")]
        public async Task<IActionResult> listAllSubcategories([FromQuery] string category)
        {
            try
            {
                var subcategories = await _categoryRepository.getAllSubcategories(category);
                return Ok(subcategories);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
