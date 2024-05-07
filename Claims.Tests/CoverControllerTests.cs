using Application.DTOs;
using Domain.Entities;
using Domain.Types;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Xunit;

namespace Claims.Tests;

[Collection("Integration")]
public class CoverControllerTests : 
             IClassFixture<TestWebApplicationFactory>, IDisposable
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly DateTime _today = DateTime.Today;

    public CoverControllerTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Get_Covers()
    {
        // Arrange
        var dataIn = CreateDataIn();
        await _factory.GetCoverCollection.InsertManyAsync(dataIn);

        // Act
        var response = await _client.GetAsync("/Covers");

        // Assert
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var dataOut = JsonConvert.DeserializeObject<IEnumerable<CoverDTO>>(json);

        Assert.NotNull(dataOut);
        Assert.Equal(3, dataOut.Count());
        Assert.Collection(dataOut, r =>
        {
            Assert.Equal(dataIn[0].StartDate, r.StartDate);
            Assert.Equal(dataIn[0].EndDate, r.EndDate);
            Assert.Equal(dataIn[0].Type, r.Type);
            Assert.Equal(dataIn[0].Premium, r.Premium);
            Assert.NotNull(r.Id);
        },
        r =>
        {
            Assert.Equal(dataIn[1].StartDate, r.StartDate);
            Assert.Equal(dataIn[1].EndDate, r.EndDate);
            Assert.Equal(dataIn[1].Type, r.Type);
            Assert.Equal(dataIn[1].Premium, r.Premium);
            Assert.NotNull(r.Id);
        },
        r =>
        {
            Assert.Equal(dataIn[2].StartDate, r.StartDate);
            Assert.Equal(dataIn[2].EndDate, r.EndDate);
            Assert.Equal(dataIn[2].Type, r.Type);
            Assert.Equal(dataIn[2].Premium, r.Premium);
            Assert.NotNull(r.Id);
        });
    }

    [Fact]
    public async Task Get_Cover()
    {
        // Arrange
        var dataIn = CreateDataIn();
        await _factory.GetCoverCollection.InsertManyAsync(dataIn);

        // Act
        var response = await _client.GetAsync($"/Covers/{dataIn[1].Id}");

        // Assert
        var json = await response.Content.ReadAsStringAsync();
        var dataOut = JsonConvert.DeserializeObject<CoverDTO>(json);
        response.EnsureSuccessStatusCode();
        Assert.NotNull(dataOut);
        Assert.Equal(dataIn[1].Id.ToString(), dataOut.Id);
        Assert.Equal(dataIn[1].Type, dataOut.Type);
        Assert.Equal(dataIn[1].StartDate, dataOut.StartDate);
        Assert.Equal(dataIn[1].EndDate, dataOut.EndDate);
        Assert.Equal(dataIn[1].Premium, dataOut.Premium);
    }

    [Fact]
    public async Task Create_Cover()
    {
        // Arrange
        var dataIn = new CoverDTO
        {
            StartDate = _today.AddDays(5),
            EndDate = _today.AddDays(225),
            Type = CoverType.BulkCarrier
        };

        // Act
        var response = await _client.PostAsJsonAsync("/Covers", dataIn);

        // Assert
        var json = await response.Content.ReadAsStringAsync();
        var dataOut = JsonConvert.DeserializeObject<CoverDTO>(json);
        response.EnsureSuccessStatusCode();
        Assert.NotNull(dataOut);
        Assert.NotNull(dataOut.Id);
        Assert.Equal(dataIn.Type, dataOut.Type);
        Assert.Equal(dataIn.StartDate, dataOut.StartDate);
        Assert.Equal(dataIn.EndDate, dataOut.EndDate);
    }

    [Fact]
    public async Task Delete_Covers()
    {
        // Arrange
        var dataIn = CreateDataIn();
        await _factory.GetCoverCollection.InsertManyAsync(dataIn);

        // Act
        var response = await _client.DeleteAsync($"/Covers/{dataIn[1].Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var existing = await _factory.GetCoverCollection.Aggregate().ToListAsync();

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

    private List<Cover> CreateDataIn()
    {
        return new List<Cover>
            {
                new Cover
                {
                    StartDate = _today.AddDays(1),
                    EndDate = _today.AddDays(100),
                    Premium = 1000,
                    Type = CoverType.Yacht
                },
                new Cover
                {
                    StartDate = _today.AddDays(2),
                    EndDate = _today.AddDays(200),
                    Premium = 2000,
                    Type = CoverType.Tanker
                },
                new Cover
                {
                    StartDate = _today.AddDays(3),
                    EndDate = _today.AddDays(300),
                    Premium = 3000,
                    Type = CoverType.ContainerShip
                }
            };
    }

    public void Dispose()
    {
        _factory.GetCoverCollection.DeleteManyAsync(_ => true).Wait();
    }
}
