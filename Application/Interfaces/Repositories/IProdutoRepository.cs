using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Repositories;

public interface IProdutoRepository
{
    Task<Produto?> Obter(Guid id, CancellationToken cancellationToken = default);
    Task<Produto?> ObterPorCodigo(string codigo, CancellationToken cancellationToken = default);
    Task Criar(Produto produto, CancellationToken cancellationToken = default);
    void Editar(Produto produto);
    void Deletar(Produto produto);
}