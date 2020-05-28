using System.Threading.Tasks;
using Crypto.Application.Tickets.Queries;
using NUnit.Framework;

namespace Crypto.Application.IntegrationTests.Tickets.Queries
{
    using static Testing;
    
    public class GetTicketDetailsTests : TestBase
    {
        [Test]
        public async Task ShouldReturnDetails()
        {
            var query = new GetTicketDetailsQuery{ Id = 5};
            var result = await SendAsync(query);
        }
    }
}