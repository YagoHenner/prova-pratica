using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProdutoRepository(LojasHennerDbContext context) : IProdutoRepository
{
    public async Task<Produto?> Obter(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Produtos.FindAsync([id], cancellationToken);
    }
    public async Task<List<Produto>> Obter(
        string? categoria,
        decimal? precoMinimo,
        decimal? precoMaximo,
        StatusProdutoEnum? status,
        int pagina,
        int tamanhoPagina,
        CancellationToken cancellationToken = default)
    {
        var query = AplicarFiltros(categoria, precoMinimo, precoMaximo, status);

        return await query
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)  
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<int> ContarTotalPorFiltros(
        string? categoria,
        decimal? precoMinimo,
        decimal? precoMaximo,
        StatusProdutoEnum? status,
        CancellationToken cancellationToken = default)
    {
        var query = AplicarFiltros(categoria, precoMinimo, precoMaximo, status);
        return await query.CountAsync(cancellationToken);
    }


    /// <summary>
    /// Método privado para centralizar a lógica de filtragem
    /// </summary>
    private IQueryable<Produto> AplicarFiltros(
        string? categoria,
        decimal? precoMinimo,
        decimal? precoMaximo,
        StatusProdutoEnum? status)
    {
        var query = context.Produtos.AsQueryable();

        if (!string.IsNullOrWhiteSpace(categoria))
        {
            query = query.Where(p => p.Categoria.ToLower() == categoria.ToLower());
        }

        if (precoMinimo.HasValue)
        {
            query = query.Where(p => p.Preco >= precoMinimo.Value);
        }

        if (precoMaximo.HasValue)
        {
            query = query.Where(p => p.Preco <= precoMaximo.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(p => p.Status == status.Value);
        }

        return query;
    }

    public async Task<Produto?> ObterPorCodigo(string codigo, CancellationToken cancellationToken = default)
    {
        return await context.Produtos.FirstOrDefaultAsync(p => p.Codigo == codigo, cancellationToken);
    }
    public async Task Criar(Produto produto, CancellationToken cancellationToken = default)
    {
        await context.Produtos.AddAsync(produto, cancellationToken);
    }

    public void Editar(Produto produto)
    {
        context.Entry(produto).State = EntityState.Modified;
    }

    public void Deletar(Produto produto)
    {
        context.Produtos.Remove(produto);
    }
}