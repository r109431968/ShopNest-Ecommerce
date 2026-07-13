using MediatR;
using ShopNest.Application.DTOs.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopNest.Application.Features.Cart.Queries
{
    public class GetCartQuery : IRequest<CartResponseDto>
    {
        public int UserId { get; set; }

        public GetCartQuery(int userId)
        {
            UserId = userId;
        }
    }
}
