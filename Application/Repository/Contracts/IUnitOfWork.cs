namespace Application.Repository.Contracts;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}
