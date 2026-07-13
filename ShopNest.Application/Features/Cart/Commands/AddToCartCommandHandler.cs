using MediatR;
using ShopNest.Application.Interfaces;
using ShopNest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopNest.Application.Features.Cart.Commands
{
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddToCartCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            // Product validation
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new Exception("Product Not found!");
            }

            // Step 1: User ka Cart dhoondo
            var existingCarts = await _unitOfWork.Carts.FindAsync(c => c.UserId == request.UserId);
            var cart = existingCarts.FirstOrDefault();

            // Step 2: Agar Cart nahi mila, naya banao
            if (cart == null)
            {
                cart = new Domain.Entities.Cart
                {
                    UserId = request.UserId,
                    CreatedAt = DateTime.UtcNow
                };
                await _unitOfWork.Carts.AddAsync(cart);
                await _unitOfWork.SaveChangesAsync();   // taaki cart.Id mil jaye
            }

            // Step 3: Check karo, ye Product already CartItem hai kya isi Cart mein
            var existingItems = await _unitOfWork.CartItems.FindAsync(
                ci => ci.CartId == cart.Id && ci.ProductId == request.ProductId);
            var cartItem = existingItems.FirstOrDefault();

            int totalRequestedQuantity = (cartItem?.Quantity ?? 0) + request.Quantity;

            if (totalRequestedQuantity > product.Quantity)
            {
                throw new Exception("Requested quantity exceeds available stock.");
            }

            if (cartItem != null)
            {
                // Step 4a: Already hai, Quantity badhao
                cartItem.Quantity += request.Quantity;
                _unitOfWork.CartItems.Update(cartItem);
            }
            else
            {
                // Step 4b: Naya CartItem banao
                cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                };
                await _unitOfWork.CartItems.AddAsync(cartItem);
            }

            await _unitOfWork.SaveChangesAsync();

            return cartItem.Id;
        }
    }
}
