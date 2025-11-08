using Application.Interfaces.Repositories;
using Application.Services.ErrorHandling;
using FluentResults;
using MediatR;

namespace Application.UseCases.Produtos.EditarProduto;

public class EditarProdutoCommandHandler(
    IProdutoRepository produtoRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<EditarProdutoCommand, Result>
{
    public async Task<Result> Handle(EditarProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = await produtoRepository.Obter(request.Id, cancellationToken);

        if (produto is null)
            return new UnprocessableEntityError("Produto não encontrado.");

        produto.Editar(
            request.Nome,
            request.Descricao,
            request.Preco,
            request.Categoria
        );

        produtoRepository.Editar(produto);

        await unitOfWork.SalvarAlteracoes(cancellationToken);

        return Result.Ok();
    }
}