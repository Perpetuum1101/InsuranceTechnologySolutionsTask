using Domain.Entities.Contracts;
using Domain.Types;

namespace Domain.Entities;

public class Claim : IHasId
{
    public Guid? Id { get; set; }

    public string CoverId { get; set; } = null!;

    public DateTime Created { get; set; }

    public string Name { get; set; } = null!;

    public ClaimType Type { get; set; }

    public decimal DamageCost { get; set; }
}