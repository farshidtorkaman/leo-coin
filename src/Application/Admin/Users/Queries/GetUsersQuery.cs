using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Interfaces;
using MD.PersianDateTime.Standard;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Admin.Users.Queries
{
    public class GetUsersQuery : IRequest<List<UserVm>>
    {
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserVm>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public GetUsersQueryHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<List<UserVm>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _identityService.GetUsers();
            if (!users.Any()) return new List<UserVm>();

            var usersVm = new List<UserVm>();
            foreach (var userId in users)
            {
                var userVm = new UserVm {Id = userId};

                var profile =
                    await _context.UsersProfiles.FirstOrDefaultAsync(f => f.UserId == userId, cancellationToken);
                userVm.FullName = profile.FirstName + " " + profile.LastName;
                userVm.PhoneNumber = profile.PhoneNumber ?? "-";
                userVm.Tell = profile.Tell ?? "-";

                var document =
                    await _context.Documents.FirstOrDefaultAsync(f => f.UserId == userId, cancellationToken);
                userVm.NationalCode = document == null ? "-" : document.NationalCode;
                userVm.BirthDate = document?.BirthDate != null
                    ? new PersianDateTime(document.BirthDate).ToShortDateString()
                    : "-";


                usersVm.Add(userVm);
            }

            return usersVm;
        }
    }
}