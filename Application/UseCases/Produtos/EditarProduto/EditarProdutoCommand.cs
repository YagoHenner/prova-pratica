using FluentResults;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.UseCases.Produtos.EditarProduto;

/// <summary>
/// Command para atualizar um produto.
/// </summary>
public class EditarProdutoCommand : IRequest<Result>
{
    [JsonIgnore]
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public decimal Preco { get; private set; }
    public string Categoria { get; private set; }

    public EditarProdutoCommand(string nome, string descricao, decimal preco, string categoria)
    {
        Nome = nome;
        Descricao = descricao;
        Preco = preco;
        Categoria = categoria;
    }

    /// <summary>
    /// Método auxiliar para injetar o ID da rota no command.
    /// </summary>
    public void AtribuirId(Guid id)
    {
        Id = id;
    }
}