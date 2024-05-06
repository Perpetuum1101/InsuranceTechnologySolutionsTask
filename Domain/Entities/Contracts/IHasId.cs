namespace Domain.Entities.Contracts;

public interface IHasId
{
    Guid? Id { get; set; }
}
