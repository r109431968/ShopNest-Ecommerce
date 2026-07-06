using MediatR;
using ShopNest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopNest.Application.Features.Products.Queries
{
    public class GetProductByIdQueries : IRequest<Product>
    {
        public int Id { get; set; }

        public GetProductByIdQueries(int id) 
        {
            Id = id;
        }
    }
}
