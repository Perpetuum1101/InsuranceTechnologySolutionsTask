using Domain.Entities;
using Domain.Types;
using Xunit;
using Application.DTOs;
using Newtonsoft.Json;
using MongoDB.Driver;
using System.Net.Http.Json;

namespace Claims.Tests;

[Collection("Integration")]
public class ClaimsControllerTests : IClassFixture<TestWebApplicationFactory>, IDisposable
{

    private readonly TestWebApplicationFactory _factory;
    private readonly DateTime _today = DateTime.Today;
    private readonly HttpClient _client;
    

    public ClaimsControllerTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Get_Claims()
    {
        // Arrange
        var dataIn = CreateDataIn();
        await _factory.ClaimCollection.InsertManyAsync(dataIn);

        // Act
        var response = await _client.GetAsync("/Claims");

        // Assert
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var dataOut = JsonConvert.DeserializeObject<IEnumerable<ClaimDTO>>(json);

        Assert.NotNull(dataOut);
        Assert.Equal(3, dataOut.Count());
        Assert.Collection(dataOut, r =>
        {
            Assert.Equal(dataIn[0].CoverId, r.CoverId.ToString());
            Assert.Equal(dataIn[0].Created, r.Created);
            Assert.Equal(dataIn[0].Type, r.Type);
            Assert.Equal(dataIn[0].Name, r.Name);
            Assert.Equal(dataIn[0].DamageCost, r.DamageCost);
            Assert.NotNull(r.Id);
        },
        r =>
        {
            Assert.Equal(dataIn[1].CoverId, r.CoverId.ToString());
            Assert.Equal(dataIn[1].Created, r.Created);
            Assert.Equal(dataIn[1].Type, r.Type);
            Assert.Equal(dataIn[1].Name, r.Name);
            Assert.Equal(dataIn[1].DamageCost, r.DamageCost);
            Assert.NotNull(r.Id);
        },
        r =>
        {
            Assert.Equal(dataIn[2].CoverId, r.CoverId.ToString());
            Assert.Equal(dataIn[2].Created, r.Created);
            Assert.Equal(dataIn[2].Type, r.Type);
            Assert.Equal(dataIn[2].Name, r.Name);
            Assert.Equal(dataIn[2].DamageCost, r.DamageCost);
            Assert.NotNull(r.Id);
        });
    }

    [Fact]
    public async Task Delete_Claim()
    {
        // Arrange
        var dataIn = CreateDataIn();
        await _factory.ClaimCollection.InsertManyAsync(dataIn);

        // Act
        var response = await _client.DeleteAsync($"/Claims/{dataIn[1].Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var existing = await _factory.ClaimCollection.Aggregate().ToListAsync();

        Assert.Equal(2, existing.Count());
        Assert.Collection(existing, r =>
        {
            Assert.Equal(dataIn[0].Id, r.Id);
        },
        r =>
        {
            Assert.Equal(dataIn[2].Id, r.Id);
        });
    }

    [Fact]
    public async Task Get_Claim()
    {
        // Arrange
        var dataIn = CreateDataIn();
        await _factory.ClaimCollection.InsertManyAsync(dataIn);

        // Act
        var response = await _client.GetAsync($"/Claims/{dataIn[1].Id}");

        // Assert
        var json = await response.Content.ReadAsStringAsync();
        var dataOut = JsonConvert.DeserializeObject<ClaimDTO>(json);
        response.EnsureSuccessStatusCode();
        Assert.NotNull(dataOut);
        Assert.Equal(dataIn[1].Id.ToString(), dataOut.Id);
        Assert.Equal(dataIn[1].Name, dataOut.Name);
        Assert.Equal(dataIn[1].DamageCost, dataOut.DamageCost);
        Assert.Equal(dataIn[1].CoverId, dataOut.CoverId.ToString());
        Assert.Equal(dataIn[1].Created, dataOut.Created);
        Assert.Equal(dataIn[1].Type, dataOut.Type);
    }

    [Fact]
    public async Task Create_Claim()
    {
        // Arrange
        
        var cover = new Cover
        {
            StartDate = _today.AddDays(1),
            EndDate = _today.AddDays(100),
            Type = CoverType.BulkCarrier,
            Premium = 10000
        };
        await _factory.GetCoverCollection.InsertOneAsync(cover);
        var dataIn = new ClaimDTO(
                         null,
                         cover.Id!.Value,
                         _today.AddDays(99),
                         "Test1",
                         ClaimType.Collision, 10000);


        // Act
        var response = await _client.PostAsJsonAsync("/Claims", dataIn);

        // Assert
        var json = await response.Content.ReadAsStringAsync();
        var dataOut = JsonConvert.DeserializeObject<ClaimDTO>(json);
        response.EnsureSuccessStatusCode();
        Assert.NotNull(dataOut);
        Assert.NotNull(dataOut.Id);
        Assert.Equal(dataIn.CoverId, dataOut.CoverId);
        Assert.Equal(dataIn.Created, dataOut.Created);
        Assert.Equal(dataIn.Name, dataOut.Name);
        Assert.Equal(dataIn.DamageCost, dataOut.DamageCost);
        Assert.Equal(dataIn.Type, dataOut.Type);
    }

    private List<Claim> CreateDataIn()
    {
        return new List<Claim>
        {
            new Claim
            {
                Type = ClaimType.Grounding,
                Created = _today,
                DamageCost = 100000,
                Name = "Test1",
                CoverId = Guid.NewGuid().ToString()
            },
            new Claim
            {
                Type = ClaimType.Fire,
                Created = _today.AddDays(10),
                DamageCost = 99999,
                Name = "Test2",
                CoverId = Guid.NewGuid().ToString()
            },
            new Claim
            {
                Type = ClaimType.Collision,
                Created = _today.AddDays(20),
                DamageCost = 77777,
                Name = "Test3",
                CoverId = Guid.NewGuid().ToString()
            }
        };
    }

    public void Dispose()
    {
        _factory.ClaimCollection.DeleteManyAsync(_ => true).Wait();
        _factory.GetCoverCollection.DeleteManyAsync(_ => true).Wait();
    }
}
