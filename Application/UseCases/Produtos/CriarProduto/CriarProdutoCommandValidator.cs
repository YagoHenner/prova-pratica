using FluentValidation;

namespace Application.UseCases.Produtos.CriarProduto;

public class CriarProdutoCommandValidator : AbstractValidator<CriarProdutoCommand>
{
    public CriarProdutoCommandValidator()
    {
        RuleFor(x => x.Codigo)
            .NotEmpty().WithMessage("Código é obrigatório.")
            .MaximumLength(50).WithMessage("Código deve ter no máximo 50 caracteres.");

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Preco)
            .GreaterThan(0).WithMessage("Preço deve ser maior que zero.");

        RuleFor(x => x.Categoria)
            .NotEmpty().WithMessage("Categoria é obrigatória.");
    }
}