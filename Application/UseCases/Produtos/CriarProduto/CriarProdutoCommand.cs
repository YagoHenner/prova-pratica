using FluentResults;
using MediatR;

namespace Application.UseCases.Produtos.CriarProduto;

/// <summary>
/// Command para criar um novo produto
/// </summary>
public record CriarProdutoCommand(
    string Codigo,
    string Nome,
    string Descricao,
    decimal Preco,
    string Categoria) : IRequest<Result<CriarProdutoResponse>>;

/// <summary>
/// Resposta do comando de criação de produto
/// </summary>
public record CriarProdutoResponse(
    Guid Id
);