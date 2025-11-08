using Application.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class UnitOfWork(LojasHennerDbContext context) : IUnitOfWork
{
    public Task<int> SalvarAlteracoes(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}