using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.Json;
using TalentConsulting.TalentSuite.Clients.Common;
using TalentConsulting.TalentSuite.Clients.Common.Entities;
using TalentConsulting.TalentSuite.Clients.Core.Entities;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TalentConsulting.TalentSuite.Clients.FunctionalTests;

[Collection("Sequential")]
public class WhenUsingClientsApiUnitTests : BaseWhenUsingApiUnitTests
{
    [Fact]
    public async Task ThenClientsAreRetrieved()
    {
        if (!IsRunningLocally() || _client == null)
        {
            // Skip the test if not running locally
            Assert.True(true, "Test skipped because it is not running locally.");
            return;
        }

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + "api/clients?pageNumber=1&pageSize=10"),
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        await response.Content.ReadAsStringAsync();

        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<ClientDto>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        ArgumentNullException.ThrowIfNull(retVal);
        retVal.Should().NotBeNull();
        retVal.Items.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task ThenGetClientById_IsRetrieved()
    {
        if (!IsRunningLocally() || _client == null)
        {
            // Skip the test if not running locally
            Assert.True(true, "Test skipped because it is not running locally.");
            return;
        }

        var expectedClient = new ClientDto("83c756a8-ff87-48be-a862-096678b41817", "Harry Potter", "DfE", "harry@potter.com", new List<ClientProjectDto>() { new ClientProjectDto("83c756a8-ff87-48be-a862-096678b41817", "83c756a8-ff87-48be-a862-096678b41817", "86b610ee-e866-4749-9f10-4a5c59e96f2f") });

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + $"api/client/{expectedClient.Id}"),
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        await response.Content.ReadAsStringAsync();

        var retVal = await JsonSerializer.DeserializeAsync<ClientDto>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        ArgumentNullException.ThrowIfNull(retVal);
        retVal.Should().NotBeNull();
        retVal.Should().BeEquivalentTo(expectedClient);
    }

    [Fact]
    public async Task ThenTheClientIsCreated()
    {
        if (!IsRunningLocally() || _client == null)
        {
            // Skip the test if not running locally
            Assert.True(true, "Test skipped because it is not running locally.");
            return;
        }

        var report = GetTestClientDto(Guid.NewGuid().ToString());

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_client.BaseAddress + "api/client"),
            Content = new StringContent(JsonConvert.SerializeObject(report), Encoding.UTF8, "application/json"),
        };

#if ADD_BEARER_TOKEN
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");
#endif


        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var stringResult = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stringResult.Should().Be(report.Id);
    }

    [Fact]
    public async Task ThenTheClientIsUpdated()
    {
        if (!IsRunningLocally() || _client == null)
        {
            // Skip the test if not running locally
            Assert.True(true, "Test skipped because it is not running locally.");
            return;
        }

        var id = Guid.NewGuid().ToString();
        var report = GetTestClientDto(id);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_client.BaseAddress + "api/client"),
            Content = new StringContent(JsonConvert.SerializeObject(report), Encoding.UTF8, "application/json"),
        };

#if ADD_BEARER_TOKEN
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");
#endif

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        await response.Content.ReadAsStringAsync();

        var updatedreport = new ClientDto(id, "Name 1", "Contact Name 1", "Contact Email 1", new List<ClientProjectDto>() { new ClientProjectDto(_clientProjectId, _clientId, _projectId) });

        var updaterequest = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri(_client.BaseAddress + $"api/client/{updatedreport.Id}"),
            Content = new StringContent(JsonConvert.SerializeObject(updatedreport), Encoding.UTF8, "application/json"),
        };

#if ADD_BEARER_TOKEN
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");
#endif

        using var updateresponse = await _client.SendAsync(updaterequest);

        updateresponse.EnsureSuccessStatusCode();

        var updateStringResult = await updateresponse.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        updateStringResult.Should().Be(updatedreport.Id);
    }
    

    [Fact]
    public async Task ThenTheClientIsDeleted()
    {
        if (!IsRunningLocally() || _client == null)
        {
            // Skip the test if not running locally
            Assert.True(true, "Test skipped because it is not running locally.");
            return;
        }

        var id = Guid.NewGuid().ToString();
        var report = GetTestClientDto(id);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_client.BaseAddress + "api/client"),
            Content = new StringContent(JsonConvert.SerializeObject(report), Encoding.UTF8, "application/json"),
        };

#if ADD_BEARER_TOKEN
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");
#endif

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        await response.Content.ReadAsStringAsync();

        var deleterequest = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri(_client.BaseAddress + $"api/client/{id}"),
        };

#if ADD_BEARER_TOKEN
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");
#endif

        using var deletedresponse = await _client.SendAsync(deleterequest);

        deletedresponse.EnsureSuccessStatusCode();

        var deletedStringResult = await deletedresponse.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        deletedStringResult.Should().Be("true");
    }
    

    public static ClientDto GetTestClientDto(string clientId)
    {
        return new ClientDto(clientId, "Name", "Contact Name", "Contact Email", new List<ClientProjectDto>() { new ClientProjectDto(_clientProjectId, _clientId, _projectId) });
    }
}
