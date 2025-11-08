namespace Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    Task<int> SalvarAlteracoes(CancellationToken cancellationToken = default);
}