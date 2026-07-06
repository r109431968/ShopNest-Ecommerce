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
    public class GetAllProductQueriesHandler : IRequestHandler<GetAllProductQueries, IEnumerable<Product>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllProductQueriesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Product>>
            Handle(GetAllProductQueries request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Products.GetAllAsync();
        }
    }
}
