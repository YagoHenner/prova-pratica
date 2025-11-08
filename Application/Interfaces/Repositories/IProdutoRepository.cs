using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Repositories;

public interface IProdutoRepository
{
    Task<Produto?> ObterPorCodigo(string codigo, CancellationToken cancellationToken = default);
    Task Criar(Produto produto, CancellationToken cancellationToken = default);
}