using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Repositories;

public interface IProdutoRepository
{
    Task<Produto?> Obter(Guid id, CancellationToken cancellationToken = default);
    Task<List<Produto>> Obter(
        string? categoria,
        decimal? precoMinimo,
        decimal? precoMaximo,
        StatusProdutoEnum? status,
        int pagina,
        int tamanhoPagina,
        CancellationToken cancellationToken = default);

    Task<Produto?> ObterPorCodigo(string codigo, CancellationToken cancellationToken = default);

    Task<int> ContarTotalPorFiltros(
    string? categoria,
    decimal? precoMinimo,
    decimal? precoMaximo,
    StatusProdutoEnum? status,
    CancellationToken cancellationToken = default);
    Task Criar(Produto produto, CancellationToken cancellationToken = default);
    void Editar(Produto produto);
    void Deletar(Produto produto);
}