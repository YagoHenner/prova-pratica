using Domain.Enums;

namespace Domain.Entities;

public class Produto
{
    public Guid Id { get; private set; }
    public string Sku { get; private set; }
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public decimal Preco { get; private set; }
    public string Categoria { get; private set; }
    public StatusProdutoEnum Status { get; private set; }
    public string? UrlFoto { get; private set; }
    public DateTime GeradoEm { get; private set; }
    public DateTime AtualizadoEm { get; private set; }

    private Produto() { }

    public Produto(string sku, string nome, string descricao, decimal preco, string categoria)
    {
        Id = Guid.NewGuid();
        Sku = sku;
        Nome = nome;
        Descricao = descricao;
        Preco = preco;
        Categoria = categoria;
        Status = StatusProdutoEnum.Ativo;
        GeradoEm = DateTime.UtcNow;
        AtualizadoEm = DateTime.UtcNow;

        ValidarProduto();
    }

    public void Update(string name, string description, decimal price, string category)
    {
        Nome = name;
        Descricao = description;
        Preco = price;
        Categoria = category;
        AtualizadoEm = DateTime.UtcNow;

        ValidarProduto();
    }

    public void Desativar()
    {
        Status = StatusProdutoEnum.Inativo;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void Ativar()
    {
        Status = StatusProdutoEnum.Ativo;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void AtribuirUrlFoto(string url)
    {
        UrlFoto = url;
        AtualizadoEm = DateTime.UtcNow;
    }

    private void ValidarProduto()
    {
        if (string.IsNullOrWhiteSpace(Sku))
            throw new ArgumentException("SKU é obrigatório.");
        if (string.IsNullOrWhiteSpace(Nome))
            throw new ArgumentException("Nome é obrigatório.");
        if (Preco <= 0)
            throw new ArgumentException("Preço deve ser positivo.");
    }
}