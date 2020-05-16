using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using MediatR;

namespace Crypto.Application.Users.Profile.Commands
{
    public class UpdateProfileCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Tell { get; set; }

        public int? ProvinceId { get; set; }

        public int? CityId { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }
    }

    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, bool>
    {
        private readonly IApplicationDbContext _context;


        public UpdateProfileCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = await _context.UsersProfiles.FindAsync(request.Id);
            if (profile == null)
                throw new NotFoundException(nameof(UserProfile), request.Id);

            profile.FirstName = request.FirstName;
            profile.LastName = request.LastName;
            profile.PhoneNumber = request.PhoneNumber;
            profile.Tell = request.Tell;
            profile.ProvinceId = request.ProvinceId == 0 ? null : request.ProvinceId;
            profile.CityId = request.CityId == 0 ? null : request.CityId;
            profile.Address = request.Address;
            profile.PostalCode = request.PostalCode;

            await _context.SaveChangesAsync(cancellationToken);

            // if phone number has changed this will return true
            return request.PhoneNumber != null && request.PhoneNumber != profile.PhoneNumber;
        }
    }
}
