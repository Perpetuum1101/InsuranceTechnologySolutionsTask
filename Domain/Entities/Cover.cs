using Domain.Entities.Contracts;
using Domain.Types;

namespace Domain.Entities;

public class Cover : IHasId
{
    public Guid? Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public CoverType Type { get; set; }

    public decimal Premium { get; set; }
}
