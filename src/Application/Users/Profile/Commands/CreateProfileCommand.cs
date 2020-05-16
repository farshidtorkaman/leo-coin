using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using MediatR;

namespace Crypto.Application.Users.Profile.Commands
{
    public class CreateProfileCommand : IRequest<int>
    {
        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateProfileCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = new UserProfile
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserId = request.UserId
            };

            _context.UsersProfiles.Add(profile);
            await _context.SaveChangesAsync(cancellationToken);

            return profile.Id;
        }
    }
}
