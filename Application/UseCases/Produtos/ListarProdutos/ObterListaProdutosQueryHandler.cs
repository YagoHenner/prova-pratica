using Application.Interfaces.Repositories;
using FluentResults;
using Humanizer;
using MediatR;

namespace Application.UseCases.Produtos.ListarProdutos;

public class ObterListaProdutosQueryHandler(
    IProdutoRepository produtoRepository) : IRequestHandler<ObterListaProdutosQuery, Result<ObterListaProdutosResponse>>
{
    private const int TAMANHO_MAXIMO_PAGINA = 50;
    private const int TAMANHO_PADRAO_PAGINA = 10;
    private const int PAGINA_PADRAO = 1;

    public async Task<Result<ObterListaProdutosResponse>> Handle(ObterListaProdutosQuery request, CancellationToken cancellationToken)
    {
        var pagina = Math.Max(request.Pagina, PAGINA_PADRAO);

        var tamanhoPagina = request.TamanhoPagina switch
        {
            <= 0 => TAMANHO_PADRAO_PAGINA,
            > TAMANHO_MAXIMO_PAGINA => TAMANHO_MAXIMO_PAGINA,
            _ => request.TamanhoPagina
        };

        var totalItens = await produtoRepository.ContarTotalPorFiltros(
            request.Categoria,
            request.PrecoMinimo,
            request.PrecoMaximo,
            request.Status,
            cancellationToken
        );

        var produtos = await produtoRepository.Obter(
            request.Categoria,
            request.PrecoMinimo,
            request.PrecoMaximo,
            request.Status,
            pagina,
            tamanhoPagina,
            cancellationToken
        );

        var dtos = produtos.Select(p => new ProdutoResumoDto(
            p.Id,
            p.Codigo,
            p.Nome,
            p.Preco,
            p.Categoria,
            p.Status.Humanize(),
            p.UrlFoto
        )).ToList();

        var totalPaginas = (int)Math.Ceiling(totalItens / (double)tamanhoPagina);
        var metadados = new PaginacaoMetadata(pagina, tamanhoPagina, totalItens, totalPaginas);

        return new ObterListaProdutosResponse(dtos, metadados);
    }
}