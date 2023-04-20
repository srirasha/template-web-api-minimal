using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Context
{
    public class ApplicationDbContextInitialiser
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Initialise()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                    await Seed();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while trying to initialize the database");
                throw;
            }
        }

        public async Task Seed()
        {
            try
            {
                await SeedUsers();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while trying to seed the database");
                throw;
            }
        }

        private async Task SeedUsers()
        {
            if (!_context.Users.Any())
            {
                User jane = new() { Name = "Jane" };
                User john = new() { Name = "John" };

                _context.Users.AddRange(jane, john);

                await _context.SaveChangesAsync();
            }
        }
    }
}