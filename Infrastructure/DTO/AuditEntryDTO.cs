using Domain.Types;

namespace Infrastructure.DTO;

internal class AuditEntryDTO
{
    public string? EntityId { get; set; } = null!;
    public string? HttpRequestType { get; set; }
    public DateTime Timestamp { get; set; }
    public Type? Type { get; set; }

}
