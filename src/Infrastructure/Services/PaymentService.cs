using System;
using System.Security.Policy;
using System.Threading.Tasks;
using Crypto.Application.Common.Interfaces;
using Crypto.Application.Purchases.Queries;
using MediatR;
using MshPay.Core;
using MshPay.Core.Models;

namespace Crypto.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPayProvider _payProvider;
        private readonly IMediator _mediator;

        public PaymentService(IPayProvider payProvider, IMediator mediator)
        {
            _payProvider = payProvider;
            _mediator = mediator;
        }

        public async Task<string> Pay(double amount, string callBackUrl)
        {
            var status = await _payProvider.AuthorizeAsync(
                new PayRequestModel(
                    amount,
                    callBackUrl)
                {
                    Description = "Description goes here...",
                    InvoiceNumber = Guid.NewGuid().ToString("D"), // use your own...
                    Mobile = "mobile number goes here"
                });
            return status.Succeeded ? status.Result.PaymentUrl : status.Errors.ToString();
        }

        public async Task<(bool result, string transactionId)> VerifyAsync(string status, string token)
        {
            var result = await _payProvider.VerifyAsync(new VerifyRequestModel(token) {Status = status});
            
            var isTransactionIdExist = await _mediator.Send(new IsAnyTransactionIdQuery
                {TransactionId = result.Result.TransactionId});
            
            return isTransactionIdExist ? (false, "") : (result.Succeeded, result.Result.TransactionId);
        }
    }
}