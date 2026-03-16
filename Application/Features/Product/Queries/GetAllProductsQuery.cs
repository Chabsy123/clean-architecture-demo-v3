using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Product.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<Domain.Entities.Product>>
    {
        internal class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Domain.Entities.Product>>
        {
            public async Task<IEnumerable<Domain.Entities.Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
            {
                //logic to get all products
                var list = new List<Domain.Entities.Product>();
                for (int i = 0; i <= 100; i++)
                {
                    var prod = new Domain.Entities.Product();
                    prod.Name = "Mobile";
                    prod.Description = "test Mobile";
                    prod.Rate = 100 + i;

                    list.Add(prod);
                }
                return list;
            }
        }
    }

}