using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services.ErrorHandling;
using FluentResults;
using MediatR;

namespace Application.UseCases.Produtos.EnviarImagemProdutoMinio;

public class EnviarFotoProdutoMinioCommandHandler(
    IProdutoRepository produtoRepository,
    IUnitOfWork unitOfWork,
    IServicoEnvioArquivosMinio servicoArmazenamento)
    : IRequestHandler<EnviarFotoProdutoMinioCommand, Result<EnviarFotoProdutoMinioResponse>>
{
    public async Task<Result<EnviarFotoProdutoMinioResponse>> Handle(EnviarFotoProdutoMinioCommand request, CancellationToken cancellationToken)
    {
        // 1. Valida se o produto existe
        var produto = await produtoRepository.Obter(request.ProdutoId, cancellationToken);
        if (produto is null)
        {
            return Result.Fail(new UnprocessableEntityError("Produto não encontrado."));
        }

        // 2. Cria um nome de arquivo único
        var extensao = Path.GetExtension(request.ArquivoFoto.FileName);
        var nomeArquivo = $"{produto.Id}{extensao}"; // Ex: guid-do-produto.jpg

        // 3. Faz o upload para o MinIO/S3
        var urlFoto = await servicoArmazenamento.UploadAsync(request.ArquivoFoto, nomeArquivo);

        // 4. Atualiza a entidade de domínio
        produto.AtribuirUrlFoto(urlFoto);
        produtoRepository.Editar(produto);

        // 5. Salva no banco
        await unitOfWork.SalvarAlteracoes(cancellationToken);

        return new EnviarFotoProdutoMinioResponse(urlFoto);
    }
}