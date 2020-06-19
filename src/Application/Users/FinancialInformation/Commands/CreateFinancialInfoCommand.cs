using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using Crypto.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Users.FinancialInformation.Commands
{
    public class CreateFinancialInfoCommand : IRequest<int>
    {
        public string CardNumber { get; set; }
        public int? BankId { get; set; }
        public string AccountNumber { get; set; }
        public string Sheba { get; set; }
        public IFormFile BankCardImage { get; set; }
    }

    public class CreateFinancialInfoCommandHandler : IRequestHandler<CreateFinancialInfoCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IImageAccessor _imageAccessor;
        private readonly ICurrentUserService _currentUserService;
        private readonly INotificationService _notificationService;

        public CreateFinancialInfoCommandHandler(IApplicationDbContext context, IImageAccessor imageAccessor,
            ICurrentUserService currentUserService, INotificationService notificationService)
        {
            _context = context;
            _imageAccessor = imageAccessor;
            _currentUserService = currentUserService;
            _notificationService = notificationService;
        }

        public async Task<int> Handle(CreateFinancialInfoCommand request, CancellationToken cancellationToken)
        {
            var financialInformationCount = await _context.FinancialInformation
                .Where(f => f.CreatedBy == _currentUserService.UserId).CountAsync(cancellationToken);
            
            var financialInfo = new FinancialInfo
            {
                CardNumber = request.CardNumber,
                BankId = request.BankId == 0 ? null : request.BankId,
                AccountNumber = request.AccountNumber,
                Sheba = request.Sheba,
                BankCardImage = await _imageAccessor.Upload(request.BankCardImage, _currentUserService.UserId,
                    "کارت بانکی " + ++financialInformationCount),
                Status = Status.Sent
            };
            _context.FinancialInformation.Add(financialInfo);
            _notificationService.SendAsync(NotificationType.BankCard);

            await _context.SaveChangesAsync(cancellationToken);

            return financialInfo.Id;
        }
    }
}