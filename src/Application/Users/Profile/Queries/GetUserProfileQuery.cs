using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crypto.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Users.Profile.Queries
{
    public class GetUserProfileQuery : IRequest<UserProfileVm>
    {
        public string UserId { get; set; }
    }

    public class GetUseProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetUseProfileQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserProfileVm> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            return await _context.UsersProfiles.Where(f => f.UserId == request.UserId)
                .ProjectTo<UserProfileVm>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(cancellationToken);
        }
    }
}
