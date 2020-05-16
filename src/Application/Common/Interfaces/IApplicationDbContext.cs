using Crypto.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Crypto.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<TodoList> TodoLists { get; set; }

        DbSet<TodoItem> TodoItems { get; set; }

        DbSet<Currency> Currencies { get; set; }

        DbSet<Province> Provinces { get; set; }

        DbSet<City> Cities { get; set; }

        DbSet<UserProfile> UsersProfiles { get; set; }
        
        DbSet<FinancialInfo> FinancialInformation { get; set; }
        
        DbSet<Bank> Banks { get; set; }
        
        DbSet<Document> Documents { get; set; }
        
        DbSet<Purchase> Purchases { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
