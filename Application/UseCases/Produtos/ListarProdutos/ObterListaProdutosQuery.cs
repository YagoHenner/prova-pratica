using Domain.Enums;
using FluentResults;
using MediatR;

namespace Application.UseCases.Produtos.ListarProdutos;

/// <summary>
/// Query para listar produtos com filtros
/// </summary>
public record ObterListaProdutosQuery(
    string? Categoria,
    decimal? PrecoMinimo,
    decimal? PrecoMaximo,
    StatusProdutoEnum? Status,
    int Pagina,
    int TamanhoPagina
) : IRequest<Result<ObterListaProdutosResponse>>;

/// <summary>
/// Resposta da listagem de produtos.
/// </summary>
public record ObterListaProdutosResponse(
    List<ProdutoResumoDto> Produtos,
    PaginacaoMetadata Paginacao
);

/// <summary>
/// Metadados da paginação
/// </summary>
public record PaginacaoMetadata(
    int PaginaAtual,
    int TamanhoPagina,
    int TotalItens,
    int TotalPaginas
);

/// <summary>
/// DTO de resumo do produto (usado na listagem).
/// </summary>
public record ProdutoResumoDto(
    Guid Id,
    string Codigo,
    string Nome,
    decimal Preco,
    string Categoria,
    string Status,
    string? UrlFoto
);