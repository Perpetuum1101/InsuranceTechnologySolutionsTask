using Domain.Entities;
using Domain.Entities.Contracts;
using Infrastructure.DTO;

namespace Infrastructure.Factory;

internal static class AuditFactory
{
    public static IAudit Create(AuditEntryDTO audit)
    {
        return audit.Type switch
        {
            Type claim when claim == typeof(Claim) => new ClaimAudit
            {
                ClaimId = audit.EntityId,
                Created = audit.Timestamp,
                HttpRequestType = audit.HttpRequestType,
            },
            Type cover when cover == typeof(Cover) => new CoverAudit
            {
                CoverId = audit.EntityId,
                Created = audit.Timestamp,
                HttpRequestType = audit.HttpRequestType,
            },
            _ => throw new Exception($"{audit.Type} type is not supported for auditing"),
        };
    }
}
