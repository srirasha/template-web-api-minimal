using Application._Common.Interfaces;
using System;

namespace Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.UtcNow;
    }
}