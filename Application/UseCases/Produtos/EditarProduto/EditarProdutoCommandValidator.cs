using FluentValidation;

namespace Application.UseCases.Produtos.EditarProduto;

public class EditarProdutoCommandValidator : AbstractValidator<EditarProdutoCommand>
{
    public EditarProdutoCommandValidator()
    {
        // Valida o ID (que é injetado pelo controller)
        RuleFor(x => x.Id).NotEmpty();

        // Valida os campos do Body
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Preco)
            .GreaterThan(0).WithMessage("Preço deve ser maior que zero.");

        RuleFor(x => x.Categoria)
            .NotEmpty().WithMessage("Categoria é obrigatória.");
    }
}