using MediatR;
using ShopNest.Application.Interfaces;
using ShopNest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopNest.Application.Features.Products.Queries
{
    public class GetProductByIdQueriesHandler : IRequestHandler<GetProductByIdQueries, Product?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductByIdQueriesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Product?> 
            Handle(GetProductByIdQueries request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Products.GetByIdAsync(request.Id);
        }
    }
}
