using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Product.Commands
{
    public class UpdateProductCommand : IRequest<ApiResponse<int>>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Rate { get; set; }

        public class UpdateProductCommandCommandHandler : IRequestHandler<UpdateProductCommand, ApiResponse<int>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IAuthenticatedUser _authenticatedUser;

            public UpdateProductCommandCommandHandler(IApplicationDbContext context, IAuthenticatedUser authenticatedUser)
            {
                _context = context;
                _authenticatedUser = authenticatedUser;
            }

            public async Task<ApiResponse<int>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            {
                var product = await _context.Products.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
                if (product == null)
                {
                    throw new ApiException("Product Not found");
                }
                product.Name = request.Name;
                product.Description = request.Description;
                product.Rate = request.Rate;
                product.ModifiedBy = _authenticatedUser.UserId;
                product.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return new ApiResponse<int>(product.Id, "Product updated successfully");
            }
        }
    }

}
