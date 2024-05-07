using Domain.Entities;
using Infrastructure;
using Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Claims.Tests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly MongoDbRunner _runner;
    private readonly MongoClient _client;
    private IMongoCollection<Claim>? _claimCollection;
    private IMongoCollection<Cover>? _coverCollection;

    public TestWebApplicationFactory()
    {
        _runner = MongoDbRunner.Start();
        _client = new MongoClient(_runner.ConnectionString);

        if (!BsonClassMap.IsClassMapRegistered(typeof(Cover)))
        {
            BsonClassMap.RegisterClassMap<Cover>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(c => c.StartDate).SetSerializer(new DateTimeSerializer(dateOnly: true));
                cm.MapMember(c => c.EndDate).SetSerializer(new DateTimeSerializer(dateOnly: true));
                cm.MapIdMember(c => c.Id).SetIdGenerator(CombGuidGenerator.Instance);
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Claim)))
        {
            BsonClassMap.RegisterClassMap<Claim>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(c => c.Created).SetSerializer(new DateTimeSerializer(dateOnly: true));
                cm.MapIdMember(c => c.Id)
                  .SetIdGenerator(CombGuidGenerator.Instance);
            });
        }
    }

    public MongoClient Client => _client;
    public IMongoCollection<Claim> ClaimCollection
    {
        get
        {
            return _claimCollection ??= GetClaimsCollection();
        }
    }

    public IMongoCollection<Cover> GetCoverCollection
    {
        get
        {
            return _coverCollection ??= GetCoversCollection();
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        
        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton(new CustomConfiguration(true));
            services.AddSingleton<IMongoClient>(_client);
            services.AddDbContext<AuditContext>(o =>
            {
                var inMemoryDbName = Guid.NewGuid().ToString();
                o.UseInMemoryDatabase(inMemoryDbName);
            });

        });

        builder.UseEnvironment("Integration_Test");
    }
    
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _runner?.Dispose();
            
        }
        base.Dispose(disposing);
    }

    private IMongoCollection<Cover> GetCoversCollection()
    {
        var db = _client.GetDatabase("Claims");
        var collection = db.GetCollection<Cover>("covers");

        return collection;
    }

    private IMongoCollection<Claim> GetClaimsCollection()
    {
        var db = _client.GetDatabase("Claims");
        var collection = db.GetCollection<Claim>("claims");

        return collection;
    }
}