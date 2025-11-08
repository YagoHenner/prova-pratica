using Application.Interfaces.Repositories;
using Application.Services.ErrorHandling;
using FluentResults;
using MediatR;

namespace Application.UseCases.Produtos.DeletarProduto;

public class DeletarProdutoCommandHandler(
    IProdutoRepository produtoRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeletarProdutoCommand, Result>
{
    public async Task<Result> Handle(DeletarProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = await produtoRepository.Obter(request.Id, cancellationToken);

        if (produto is null)
            return new UnprocessableEntityError("Produto não encontrado.");

        produtoRepository.Deletar(produto);

        await unitOfWork.SalvarAlteracoes(cancellationToken);

        return Result.Ok();
    }
}