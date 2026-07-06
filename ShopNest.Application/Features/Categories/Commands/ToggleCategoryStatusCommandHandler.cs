using MediatR;
using ShopNest.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopNest.Application.Features.Categories.Commands
{
    public class ToggleCategoryStatusCommandHandler : IRequestHandler<ToggleCategoryStatusCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ToggleCategoryStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int>
            Handle(ToggleCategoryStatusCommand request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(request.Id);

            if (category == null)
                return 0;

            category.IsActive = request.IsActive;

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync();

            return category.Id;
        }
    }
}
