using FluentResults;
using MediatR;

namespace Application.UseCases.Produtos.DeletarProduto;

/// <summary>
/// Command para deletar um produto pelo ID.
/// </summary>
public record DeletarProdutoCommand(
    Guid Id
) : IRequest<Result>;