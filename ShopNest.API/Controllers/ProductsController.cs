using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopNest.Application.Features.Categories.Commands;
using ShopNest.Application.Features.Products.Commands;
using ShopNest.Application.Features.Products.Queries;

namespace ShopNest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IMediator mediator, ILogger<ProductsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var product = await _mediator.Send(new GetAllProductQueries());
                return Ok(product);
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
                var product = await _mediator.Send(new GetProductByIdQueries(id));
                if (product == null)
                    return NotFound(new { message = $"Category with id {id} not found." });
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating product.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        {
            try
            {
                var productId = await _mediator.Send(command);
                return Ok(new { Id = productId, Message = "Product created successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating product.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductCommand command)
        {
            try
            {
                command.Id = id;
                var product = await _mediator.Send(command);
                if (product == 0)
                    return NotFound(new { message = $"Product with id {id} not found." });
                return Ok(new { Id = product, Message = "Product updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating product with id {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ToggleCategoryStatus(int id, [FromBody] ToggleProductStatusCommand command)
        {
            try
            {
                command.Id = id;
                var category = await _mediator.Send(command);
                if (category == 0)
                    return NotFound(new { message = $"Product with id {id} not found." });
                return Ok(new { Id = category, Message = "Product status updated successfully" });
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
                var product = await _mediator.Send(new DeleteCategoryCommand(id));
                if (product == 0)
                    return NotFound(new { message = $"Product with id {id} not found." });
                return Ok(new { Id = product, Message = "Product deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating product with id {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
