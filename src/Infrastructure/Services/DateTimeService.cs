using Crypto.Application.Common.Interfaces;
using System;

namespace Crypto.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
