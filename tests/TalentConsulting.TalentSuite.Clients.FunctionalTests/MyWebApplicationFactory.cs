using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TalentConsulting.TalentSuite.Clients.API;

namespace TalentConsulting.TalentSuite.Clients.FunctionalTests;

public class MyWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            IEnumerable<KeyValuePair<string, string?>>? initialData = new List<KeyValuePair<string, string?>>
            {
                new KeyValuePair<string, string?>("UseDbType", "UseInMemoryDatabase"),
            };
            config.AddInMemoryCollection(initialData);
        });

        return base.CreateHost(builder);
    }
}
