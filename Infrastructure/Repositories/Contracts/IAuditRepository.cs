using Domain.Entities;
using Infrastructure.DTO;

namespace Infrastructure.Repositories.Contracts;

internal interface IAuditRepository
{
    Task SaveAudit(List<AuditEntryDTO> auditEntries);
}
