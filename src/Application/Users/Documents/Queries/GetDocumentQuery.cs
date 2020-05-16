using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crypto.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Users.Documents.Queries
{
    public class GetDocumentQuery : IRequest<DocumentVm>
    {
        public string UserId { get; set; }
    }

    public class GetDocumentQueryHandler : IRequestHandler<GetDocumentQuery, DocumentVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetDocumentQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DocumentVm> Handle(GetDocumentQuery request, CancellationToken cancellationToken)
        {
            return await _context.Documents.Where(f => f.UserId == request.UserId)
                .ProjectTo<DocumentVm>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(cancellationToken);
        }
    }
}