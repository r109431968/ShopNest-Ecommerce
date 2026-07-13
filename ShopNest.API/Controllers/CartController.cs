using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopNest.Application.DTOs.Cart;
using ShopNest.Application.Features.Cart.Commands;
using ShopNest.Application.Features.Cart.Queries;

namespace ShopNest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : BaseController
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequestDto dto)
        {
            int userId = GetCurrentUserId();

            var command = new AddToCartCommand(userId, dto.ProductId, dto.Quantity);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            int userId = GetCurrentUserId();

            var query = new GetCartQuery(userId);
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
