using Infrastructure.Context;
using Infrastructure.DTO;
using Infrastructure.Factory;
using Infrastructure.Repositories.Contracts;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

internal class AuditRepository(
               AuditContext context,
               ILogger<AuditRepository> logger) : IAuditRepository
{
    private readonly AuditContext _context = context;
    private readonly ILogger _logger = logger;

    public async Task SaveAudit(List<AuditEntryDTO> auditEntries)
    {
        auditEntries.ForEach(x => _logger
                    .LogInformation($"Saving audit {x.EntityId} {x.HttpRequestType} {x.Type}"));
        var entries = auditEntries.Select(AuditFactory.Create);
        _context.AddRange(entries);
        await _context.SaveChangesAsync();
    }
}
