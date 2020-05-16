using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Cities.Queries
{
    public class GetCitiesByProvinceQuery : IRequest<List<CityVm>>
    {
        public int? ProvinceId { get; set; }
    }

    public class GetCitiesByProvinceQueryHandler : IRequestHandler<GetCitiesByProvinceQuery, List<CityVm>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCitiesByProvinceQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CityVm>> Handle(GetCitiesByProvinceQuery request, CancellationToken cancellationToken)
        {
            return await _context.Cities.Where(f => f.ProvinceId == request.ProvinceId)
                .ProjectTo<CityVm>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        }
    }
}