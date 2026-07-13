using MediatR;
using ShopNest.Application.DTOs.Cart;
using ShopNest.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopNest.Application.Features.Cart.Queries
{
    public class GetCartQueryHandler : IRequestHandler<GetCartQuery, CartResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCartQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CartResponseDto> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            var existingCarts = await _unitOfWork.Carts.FindAsync(c => c.UserId == request.UserId);
            var cart = existingCarts.FirstOrDefault();

            // Agar Cart hi nahi mila, khaali response do
            if (cart == null)
            {
                return new CartResponseDto
                {
                    CartId = 0,
                    Items = new List<CartItemResponseDto>(),
                    TotalAmount = 0
                };
            }

            // Cart ke saare CartItems nikaalo
            var cartItems = await _unitOfWork.CartItems.FindAsync(ci => ci.CartId == cart.Id);

            var itemDtos = new List<CartItemResponseDto>();
            decimal totalAmount = 0;

            foreach (var item in cartItems)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);

                decimal subtotal = (product?.Price ?? 0) * item.Quantity;
                totalAmount += subtotal;

                itemDtos.Add(new CartItemResponseDto
                {
                    CartItemId = item.Id,
                    ProductId = item.ProductId,
                    ProductName = product?.Name ?? "Unknown Product",
                    Price = product?.Price ?? 0,
                    Quantity = item.Quantity,
                    Subtotal = subtotal
                });
            }

            return new CartResponseDto
            {
                CartId = cart.Id,
                Items = itemDtos,
                TotalAmount = totalAmount
            };
        }
    }
}
