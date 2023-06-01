using AutoMapper;
using CarRental.Auth;
using CarRental.Dto;
using CarRental.Models;
using CarRental.Services;
using CarRental.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    
    public class CategoryController : ApiBaseController
    {
        
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly Authentication _authentication;
        private readonly IConfiguration _configuration;

        public CategoryController(ICategoryService categoryService, IMapper mapper, IConfiguration configuration) 
        { 
            _categoryService = categoryService;
            _mapper = mapper;
            _authentication = new Authentication(configuration);
            _configuration = configuration;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetCategories() 
        {
            var categories = await _categoryService.GetCategories();
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var categoryDto = _mapper.Map<List<CategoryDto>>(categories);
            return Ok(categoryDto);
        }

        [HttpPost(), Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryCreate) 
        {
        
            var categories = await _categoryService.GetCategories();
            var category = categories.
                Where(c => c.Name.Trim().ToUpper() == categoryCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault(); 

            if (category !=null ) 
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }

            var categoryMap = _mapper.Map<Category>(categoryCreate);

            if (!await _categoryService.CreateCategory(categoryMap)) 
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }


            return Ok(categoryMap);
        }

        [HttpPut("{categoryId}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int categoryId,CategoryDto updatedCategory)
        {


            if (updatedCategory == null)
            {
                return BadRequest(ModelState);
            }

            if (categoryId != updatedCategory.Id)
            {
                return BadRequest(ModelState);
            }

            if (!await _categoryService.CategoryExists(categoryId)) 
            {
                return NotFound();
            }

            var categoryMap = _mapper.Map<Category>(updatedCategory);

            if (!await _categoryService.UpdateCategory(categoryMap)) 
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Success");

        }

        [HttpDelete("{categoryId}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int categoryId) 
        {
            if (!await _categoryService.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var categoryDelete = await _categoryService.GetCategory(categoryId);

            if (!await _categoryService.DeleteCategory(categoryDelete)) 
            {
                ModelState.AddModelError("", "Something went wrong");
            }
            return Ok("Success");
        }
    }
}


// in controller fac endpointuri pe care le voi folosi in frontend (post, get, put, delete)
