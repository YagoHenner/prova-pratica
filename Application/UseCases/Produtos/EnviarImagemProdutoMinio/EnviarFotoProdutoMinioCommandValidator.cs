using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Produtos.EnviarImagemProdutoMinio;

public class EnviarFotoProdutoMinioCommandValidator : AbstractValidator<EnviarFotoProdutoMinioCommand>
{
    private const long MaxTamanhoArquivo = 5 * 1024 * 1024; // 5 MB

    public EnviarFotoProdutoMinioCommandValidator()
    {
        RuleFor(x => x.ProdutoId)
            .NotEmpty().WithMessage("ID do produto é obrigatório.");

        RuleFor(x => x.ArquivoFoto)
            .NotNull().WithMessage("Nenhum arquivo enviado.")
            .Must(file => file.Length > 0).WithMessage("Arquivo vazio.")
            .Must(file => file.Length <= MaxTamanhoArquivo).WithMessage($"Arquivo excede o tamanho máximo de {MaxTamanhoArquivo / 1024 / 1024}MB.")
            .Must(ImagemValida).WithMessage("Formato de arquivo inválido. Use .jpg, .jpeg ou .png.");
    }

    private bool ImagemValida(IFormFile arquivo)
    {
        if (arquivo == null) return true; // A regra NotNull trata isso

        var extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
        return extensao == ".jpg" || extensao == ".jpeg" || extensao == ".png";
    }
}