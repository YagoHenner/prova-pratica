using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Repositories;

public interface IProdutoRepository
{
    /// <summary>
    /// Busca um produto pelo seu ID (chave primária)
    /// </summary>
    Task<Produto?> Obter(Guid id, CancellationToken cancellationToken = default);
    Task<List<Produto>> Obter(
        string? categoria,
        decimal? precoMinimo,
        decimal? precoMaximo,
        StatusProdutoEnum? status,
        int pagina,
        int tamanhoPagina,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca um produto pelo seu código
    /// </summary>
    Task<Produto?> ObterPorCodigo(string codigo, CancellationToken cancellationToken = default);

    /// <summary>
    /// Conta o número total de produtos que correspondem aos filtros (sem paginar).
    /// </summary>
    /// <returns>O número total de itens.</returns>
    Task<int> ContarTotalPorFiltros(
    string? categoria,
    decimal? precoMinimo,
    decimal? precoMaximo,
    StatusProdutoEnum? status,
    CancellationToken cancellationToken = default);

    /// <summary>
    /// Cria um novo produto no repositório
    /// </summary>
    Task Criar(Produto produto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Modifica um produto no repositório e o marca como alterado.
    /// </summary>
    void Editar(Produto produto);

    /// <summary>
    /// Deleta o produto especificado do repositório.
    /// </summary>
    void Deletar(Produto produto);
}