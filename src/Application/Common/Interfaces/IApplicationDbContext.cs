﻿using Crypto.Domain.Entities;
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
        
        DbSet<Sell> Sells { get; set; }
        
        DbSet<Ticket> Tickets { get; set; }
        
        DbSet<TicketMessage> TicketMessages { get; set; }
        
        DbSet<Notification> Notifications { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
