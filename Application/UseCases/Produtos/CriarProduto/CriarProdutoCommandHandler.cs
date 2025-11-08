using Application.Interfaces.Repositories;
using Application.Services.ErrorHandling;
using Domain.Entities;
using FluentResults;
using MediatR;

namespace Application.UseCases.Produtos.CriarProduto;

public class CriarProdutoCommandHandler(
    IProdutoRepository produtoRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CriarProdutoCommand, Result<CriarProdutoResponse>>
{
    public async Task<Result<CriarProdutoResponse>> Handle(CriarProdutoCommand request, CancellationToken cancellationToken)
    {
        var produtoExistente = await produtoRepository.ObterPorCodigo(request.Codigo, cancellationToken);

        if (produtoExistente is not null)
            return new UnprocessableEntityError("Código já cadastrado.");

        var produto = new Produto(
            request.Codigo,
            request.Nome,
            request.Descricao,
            request.Preco,
            request.Categoria
        );

        await produtoRepository.Criar(produto, cancellationToken);

        await unitOfWork.SalvarAlteracoes(cancellationToken);

        return new CriarProdutoResponse(produto.Id);
    }
}