using Crypto.Domain.Enums;

namespace Crypto.Application.Reports.Queries
{
    public class ConfirmationQueryVm
    {
        public string Title { get; set; }
        public Status Status { get; set; }
        public string ClassName { get; set; }
    }
}