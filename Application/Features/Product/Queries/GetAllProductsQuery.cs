using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Product.Queries
{
    public class GetAllProductsQuery : IRequest<ApiResponse<IEnumerable<Domain.Entities.Product>>>
    {
        public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, ApiResponse<IEnumerable<Domain.Entities.Product>>>
        {
            private readonly IApplicationDbContext _context;

            public GetAllProductsQueryHandler(IApplicationDbContext context)
            {
                //logic to get all products
                _context = context;
            }

            public async Task<ApiResponse<IEnumerable<Domain.Entities.Product>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
            {
                var result = await _context.Products.ToListAsync(cancellationToken);
                if (result == null)
                {
                    throw new ApiException("Product not found");
                }

                return new ApiResponse<IEnumerable<Domain.Entities.Product>>(result, "Data Fetched successfully");
            }
        }
    }

}