using MediatR;
using ShopNest.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopNest.Application.Features.Products.Commands
{
    public class ToggleProductStatusCommandHandler : IRequestHandler<ToggleProductStatusCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ToggleProductStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int>
            Handle(ToggleProductStatusCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.Id);

            if (product == null)
                return 0;

            product.IsActive = request.IsActive;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return product.Id;
        }
    }
}
