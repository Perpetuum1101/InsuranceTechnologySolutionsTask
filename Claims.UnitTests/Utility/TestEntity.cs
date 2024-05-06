using Domain.Entities.Contracts;

namespace Claims.UnitTests.Utility;

public class TestEntity : IHasId
{
    public Guid? Id { get; set; }
}
