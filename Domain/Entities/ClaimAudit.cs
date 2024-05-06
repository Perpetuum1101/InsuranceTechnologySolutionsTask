using Domain.Entities.Contracts;

namespace Domain.Entities;

public class ClaimAudit : IAudit
{
    public int Id { get; set; }

    public string? ClaimId { get; set; }

    public DateTime Created { get; set; }

    public string? HttpRequestType { get; set; }
}