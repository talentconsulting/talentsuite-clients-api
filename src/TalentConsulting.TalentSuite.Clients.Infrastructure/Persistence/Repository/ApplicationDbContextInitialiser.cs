using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using TalentConsulting.TalentSuite.Clients.Core.Entities;

namespace TalentConsulting.TalentSuite.Clients.Infrastructure.Persistence.Repository;

[ExcludeFromCodeCoverage]
public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync(bool isProduction, bool restartDatabase)
    {
        try
        {
            if (restartDatabase)
            {
                await _context.Database.EnsureDeletedAsync();
            }

            await InitializeDatabaseAsync();
#pragma warning disable S125
            // Only await MigrateDatabaseAsync(); when using migrations directly
#pragma warning restore S125
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    private async Task InitializeDatabaseAsync()
    {
        if (_context.Database.IsInMemory() || _context.Database.IsSqlite())
        {
            await _context.Database.EnsureCreatedAsync();
        }
    }

    //Use this if using Migrations
#pragma warning disable S1144
    private async Task MigrateDatabaseAsync()
    {
        if (_context.Database.IsSqlServer() || _context.Database.IsNpgsql())
        {

            await _context.Database.MigrateAsync();

        }
    }
#pragma warning restore S1144

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        if (_context.Clients.Any())
            return;


        if (_context.Database.IsInMemory() || _context.Database.IsSqlite())
        {
            _context.Clients.AddRange(Clients().ToArray());
        }
            
        await _context.SaveChangesAsync();
    }

    private static List<Client> Clients() 
    {
        return new List<Client>
        {
            new Client(new Guid("83c756a8-ff87-48be-a862-096678b41817"), "Harry Potter", "DfE", "harry@potter.com", new List<ClientProject>(){ new ClientProject(new Guid("83c756a8-ff87-48be-a862-096678b41817"), new Guid("519df403-0e0d-4c25-b240-8d9ca21132b8"), new Guid("86b610ee-e866-4749-9f10-4a5c59e96f2f")) } ),
            new Client(new Guid("e24a5543-6368-490a-a1d0-a18f0c69848a"), "Hermione Granger", "ESFA", "hermione@granger.com", new List<ClientProject>(){ new ClientProject(new Guid("51104a18-0e62-415b-91bc-6a0b83abceca"), new Guid("e24a5543-6368-490a-a1d0-a18f0c69848a"), new Guid("86b610ee-e866-4749-9f10-4a5c59e96f2f")) } )
        };
    }    
}
