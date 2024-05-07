using Domain.Types;
using System.Text.Json.Serialization;

namespace Application.DTOs;

public record ClaimDTO
{
    [JsonConstructor]
    public ClaimDTO(
           string? id,
           Guid coverId,
           DateTime created,
           string name,
           ClaimType type,
           decimal damageCost)
    {
        Id = id;
        CoverId = coverId;
        Created = created;
        Name = name;
        Type = type;
        DamageCost = damageCost;
    }

    public string? Id { get; set; }
    public Guid CoverId { get; set; }
    public DateTime Created { get; set; }
    public string Name { get; set; }
    public ClaimType Type { get; set; }
    public decimal DamageCost { get; set; }
}