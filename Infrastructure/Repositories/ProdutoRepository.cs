using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProdutoRepository(LojasHennerDbContext context) : IProdutoRepository
{
    public async Task<Produto?> ObterPorCodigo(string codigo, CancellationToken cancellationToken = default)
    {
        return await context.Produtos.FirstOrDefaultAsync(p => p.Codigo == codigo, cancellationToken);
    }
    public async Task Criar(Produto produto, CancellationToken cancellationToken = default)
    {
        await context.Produtos.AddAsync(produto, cancellationToken);
    }
}