using Application.UseCases.Produtos.CriarProduto;
using FluentValidation.TestHelper;
using Xunit;

namespace UseCasesTests;

/// <summary>
/// Testes para o validador CriarProdutoCommandValidator.
/// </summary>
public class CriarProdutoCommandValidatorTests
{
    private readonly CriarProdutoCommandValidator _validator;

    public CriarProdutoCommandValidatorTests()
    {
        _validator = new CriarProdutoCommandValidator();
    }

    [Fact]
    public void QuandoCamposObrigatoriosForemVazios_DeveRetornarErro()
    {
        var command = new CriarProdutoCommand(
            Codigo: string.Empty,
            Nome: string.Empty,
            Descricao: "Desc",
            Preco: 0, // Preço inválido
            Categoria: string.Empty
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Codigo);
        result.ShouldHaveValidationErrorFor(x => x.Nome);
        result.ShouldHaveValidationErrorFor(x => x.Preco);
        result.ShouldHaveValidationErrorFor(x => x.Categoria);
    }

    [Fact]
    public void QuandoCamposExcederemTamanhoMaximo_DeveRetornarErro()
    {
        var command = new CriarProdutoCommand(
            Codigo: new string('A', 51), // 51 caracteres
            Nome: new string('A', 201), // 201 caracteres
            Descricao: "Desc",
            Preco: 10,
            Categoria: "Cat"
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Codigo);
        result.ShouldHaveValidationErrorFor(x => x.Nome);
    }

    [Fact]
    public void QuandoPrecoForZeroOuNegativo_DeveRetornarErro()
    {
        var command = new CriarProdutoCommand("SKU", "Nome", "Desc", -10, "Cat");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Preco);
    }

    [Fact]
    public void QuandoCamposForemValidos_NaoDeveRetornarErro()
    {
        var command = new CriarProdutoCommand(
            Codigo: "SKU-123",
            Nome: "Produto Válido",
            Descricao: "Desc Válida",
            Preco: 10,
            Categoria: "Categoria Válida"
        );

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}