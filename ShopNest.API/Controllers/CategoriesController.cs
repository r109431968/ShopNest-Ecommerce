using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShopNest.Application.Features.Categories.Commands;
using ShopNest.Application.Features.Categories.Queries;
using ShopNest.Application.Interfaces;
using ShopNest.Domain.Entities;

namespace ShopNest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(IMediator mediator, ILogger<CategoriesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var category = await _mediator.Send(new GetAllCategoriesQuery());
                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching category.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await _mediator.Send(new GetCategoryByIdQuery(id));
                if(category == null)
                    return NotFound(new { message = $"Category with id {id} not found." });
                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating category.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
        {
            try
            {
                var categoryId = await _mediator.Send(command);
                return Ok(new { Id = categoryId, Message = "Category created successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating category.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryCommand command)
        {
            try
            {
                command.Id = id;
                var category = await _mediator.Send(command);
                if (category == 0)
                    return NotFound(new { message = $"Category with id {id} not found." });
                return Ok(new { Id = category, Message = "Category updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating category with id {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ToggleCategoryStatus(int id, [FromBody] ToggleCategoryStatusCommand command)
        {
            try
            {
                command.Id = id;
                var category = await _mediator.Send(command);
                if (category == 0)
                    return NotFound(new { message = $"Category with id {id} not found." });
                return Ok(new { Id = category, Message = "Category status updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating category with id {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _mediator.Send(new DeleteCategoryCommand(id));
                if (category == 0)
                    return NotFound(new { message = $"Category with id {id} not found." });
                return Ok(new { message = "Category deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating category with id {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
