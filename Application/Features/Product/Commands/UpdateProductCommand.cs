using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Product.Commands
{
    public class UpdateProductCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Rate { get; set; }

        internal class UpdateProductCommandCommandHandler : IRequestHandler<UpdateProductCommand, int>
        {
            private readonly IApplicationDbContext _context;

            public UpdateProductCommandCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            {
                var product = await _context.Products.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
                if (product != null)
                {
                    product.Name = request.Name;
                    product.Description = request.Description;
                    product.Rate = request.Rate;

                    await _context.SaveChangesAsync();
                    return product.Id;
                }
                return default;
            }
        }
    }

}
