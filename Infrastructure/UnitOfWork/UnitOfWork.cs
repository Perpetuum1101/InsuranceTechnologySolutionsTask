using Application.Repository.Contracts;
using Domain.Entities.Contracts;
using Hangfire;
using Infrastructure.Context;
using Infrastructure.DTO;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UnitOfWork;

internal class UnitOfWork(ClaimsContext claims) : IUnitOfWork
{
    private readonly ClaimsContext _claims = claims;

    public async Task SaveChangesAsync()
    {
        var auditEntries = GenerateAuditEntries();
        await _claims.SaveChangesAsync();
        AuditChange(auditEntries);
    }

    private static void AuditChange(List<AuditEntryDTO> entries)
    {
        if (entries.Count == 0)
        {
            return;
        }

        BackgroundJob.Enqueue<IAuditRepository>(x => x.SaveAudit(entries));
    }


    private List<AuditEntryDTO> GenerateAuditEntries()
    {
        var hasId = _claims.ChangeTracker.Entries<IHasId>().ToList();
        var result = _claims.ChangeTracker
                            .Entries<IHasId>()
                            .Where(x => x.State == EntityState.Deleted ||
                                        x.State == EntityState.Added)
                            .Select(x => new AuditEntryDTO
                            {
                                Timestamp = DateTime.Now,
                                HttpRequestType = HttpRequestTypeByEntityState(x.State),
                                EntityId = x.Entity.Id.ToString(),
                                Type = x.Entity.GetType()
                            })
                            .ToList();
        return result;
    }

    private static string? HttpRequestTypeByEntityState(EntityState entityState)
    {
        return entityState switch
        {
            EntityState.Added => "POST",
            EntityState.Deleted => "DELETE",
            _ => null,
        };
    }
}
