using System.Threading;
using System.Threading.Tasks;
using Crypto.Application.Common.Interfaces;
using Crypto.Domain.Entities;
using MediatR;

namespace Crypto.Application.Users.FinancialInformation.Commands
{
    public class UpdateFinancialInfoCommand : IRequest
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public int? BankId { get; set; }
        public string AccountOwnerName { get; set; }
        public string AccountNumber { get; set; }
        public string Sheba { get; set; }
    }

    public class UpdateFinancialInfoCommandHandler : IRequestHandler<UpdateFinancialInfoCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateFinancialInfoCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateFinancialInfoCommand request, CancellationToken cancellationToken)
        {
            var financialInfo = await _context.FinancialInformation.FindAsync(request.Id);
            if (financialInfo == null)
            {
                financialInfo = new FinancialInfo
                {
                    CardNumber = request.CardNumber,
                    BankId = request.BankId == 0 ? null : request.BankId,
                    AccountOwnerName = request.AccountOwnerName,
                    AccountNumber = request.AccountNumber,
                    Sheba = request.Sheba
                };
                _context.FinancialInformation.Add(financialInfo);
            }
            else
            {
                financialInfo.CardNumber = request.CardNumber;
                financialInfo.BankId = request.BankId == 0 ? null : request.BankId;
                financialInfo.AccountOwnerName = request.AccountOwnerName;
                financialInfo.AccountNumber = request.AccountNumber;
                financialInfo.Sheba = request.Sheba; 
            }
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}