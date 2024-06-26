﻿using Application.Repository.Contracts;
using Domain.Entities;
using Hangfire;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Infrastructure;

public static class Bootstrap
{
    public static void AddInfrastructure(
                       this IServiceCollection services, 
                       string sqlConnectionSting,
                       string mongoConnectionString,
                       string mongoDbName)
    {
        services.AddScoped<IClaimRepository, ClaimRepository>();
        services.AddScoped<ICoverRepository, CoverRepository>();
        services.AddScoped<IAuditRepository, AuditRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        services.AddSingleton(new CustomConfiguration(false));
        services.AddDbContext<AuditContext>(options => options.UseSqlServer(sqlConnectionSting));
        services.AddScoped<IMongoClient, MongoClient>(_ => new MongoClient(mongoConnectionString));
        services.AddDbContext<ClaimsContext>(
            (ser, options) =>
            {
                var client = ser.GetService<IMongoClient>()!;
                var database = client.GetDatabase(mongoDbName);
                options.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName);
            }
        );
        services.AddHangfire((sp, config) =>
        {
            config.UseSqlServerStorage(sqlConnectionSting);
        });
        services.AddHangfireServer();
    }

    public static void MigrateContext(this IServiceProvider services)
    {
        var configuration = services.GetRequiredService<CustomConfiguration>();
        if (!configuration.IsIntegrationTesting)
        {
            using (var scope = services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AuditContext>();
                if (context.Database.IsSqlServer())
                {
                    context.Database.Migrate();
                }
            }

            BsonClassMap.RegisterClassMap<Cover>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(c => c.StartDate).SetSerializer(new DateTimeSerializer(dateOnly: true));
                cm.MapMember(c => c.EndDate).SetSerializer(new DateTimeSerializer(dateOnly: true));
                cm.MapIdMember(c => c.Id)
                    .SetIdGenerator(CombGuidGenerator.Instance);
            });

            BsonClassMap.RegisterClassMap<Claim>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(c => c.Created).SetSerializer(new DateTimeSerializer(dateOnly: true));
                cm.MapIdMember(c => c.Id)
                  .SetIdGenerator(CombGuidGenerator.Instance);
            });
        }
    }
}
