using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Application.UseCases.Produtos.EnviarImagemProdutoMinio;

/// <summary>
/// Command para upload da foto de um produto.
/// Este DTO atua como um "wrapper model" para o [FromForm] binding.
/// </summary>
public class EnviarFotoProdutoMinioCommand : IRequest<Result<EnviarFotoProdutoMinioResponse>>
{
    /// <summary>
    /// O ID do produto, lido da ROTA.
    /// </summary>
    [FromRoute(Name = "id")]
    public Guid ProdutoId { get; set; }

    /// <summary>
    /// O arquivo de imagem, lido do form.
    /// </summary>
    [BindRequired]
    public IFormFile ArquivoFoto { get; set; } = null!;

    /// <summary>
    /// Construtor vazio necessário para o Model Binder.
    /// </summary>
    public EnviarFotoProdutoMinioCommand() { }
}

/// <summary>
/// Resposta do upload.
/// </summary>
public record EnviarFotoProdutoMinioResponse(
    string UrlFoto
);