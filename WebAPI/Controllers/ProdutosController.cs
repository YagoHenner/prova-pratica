using Application.UseCases.Produtos.CriarProduto;
using Application.UseCases.Produtos.DeletarProduto;
using Application.UseCases.Produtos.EditarProduto;
using Application.UseCases.Produtos.EnviarImagemProdutoMinio;
using Application.UseCases.Produtos.ListarProdutos;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.HttpResults;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProdutosController(ISender mediator, ResultSerializer resultSerializer) : ControllerBase
{
    /// <summary>
    /// Cadastra um novo produto.
    /// </summary>
    /// <param name="command">Dados do produto para cadastro.</param>
    /// <returns>O ID do produto recém-criado.</returns>
    /// <response code="201">Produto criado com sucesso.</response>
    /// <response code="400">Dados inválidos (ex: nome faltando, preço zero).</response>
    /// <response code="422">Regra de negócio violada (ex: Código/SKU já existe).</response>
    [HttpPost]
    [ProducesResponseType(typeof(CriarProdutoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IResult> CriarProduto([FromBody] CriarProdutoCommand command)
    {
        var result = await mediator.Send(command);

        return resultSerializer.Serialize(result);
    }

    /// <summary>
    /// Atualiza um produto existente.
    /// </summary>
    /// <param name="id">ID do produto a ser atualizado.</param>
    /// <param name="command">Novos dados do produto.</param>
    /// <returns>Nenhum conteúdo (204) em caso de sucesso.</returns>
    /// <response code="204">Produto atualizado com sucesso.</response>
    /// <response code="400">Dados inválidos (ex: nome faltando, preço zero).</response>
    /// <response code="422">Produto não encontrado.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IResult> AtualizarProduto(Guid id, [FromBody] EditarProdutoCommand command)
    {
        command.AtribuirId(id);
        var result = await mediator.Send(command);
        return resultSerializer.Serialize(result);
    }

    /// <summary>
    /// Exclui um produto.
    /// </summary>
    /// <param name="id">ID do produto a ser excluído.</param>
    /// <returns>Nenhum conteúdo (204) em caso de sucesso.</returns>
    /// <response code="204">Produto excluído com sucesso.</response>
    /// <response code="422">Produto não encontrado (ou falha).</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IResult> DeletarProduto(Guid id)
    {
        var result = await mediator.Send(new DeletarProdutoCommand(id));
        return resultSerializer.Serialize(result);
    }

    /// <summary>
    /// Lista os produtos com base em filtros e paginação.
    /// </summary>
    /// <param name="categoria">Filtra por categoria exata.</param>
    /// <param name="precoMinimo">Filtra por preço mínimo.</param>
    /// <param name="precoMaximo">Filtra por preço máximo.</param>
    /// <param name="status">Filtra por status (0 = Inativo, 1 = Ativo).</param>
    /// <param name="pagina">Número da página (Padrão: 1).</param>
    /// <param name="tamanhoPagina">Itens por página (Padrão: 10, Máx: 50).</param>
    /// <returns>Uma lista paginada de produtos e metadados de paginação.</returns>
    /// <response code="200">Retorna a lista de produtos (pode ser vazia).</response>
    [HttpGet]
    [ProducesResponseType(typeof(ObterListaProdutosResponse), StatusCodes.Status200OK)]
    public async Task<IResult> ObterListaProdutos(
        [FromQuery] string? categoria,
        [FromQuery] decimal? precoMinimo,
        [FromQuery] decimal? precoMaximo,
        [FromQuery] StatusProdutoEnum? status,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanhoPagina = 10)
    {
        var query = new ObterListaProdutosQuery(categoria, precoMinimo, precoMaximo, status, pagina, tamanhoPagina);
        var result = await mediator.Send(query);
        return resultSerializer.Serialize(result);
    }

    /// <summary>
    /// Faz upload da imagem de um produto.
    /// </summary>
    /// <param name="command">Contém o ID (da rota) e o Arquivo (do formulário).</param>
    /// <returns>A URL pública da imagem salva.</returns>
    /// <response code="200">Upload com sucesso.</response>
    /// <response code="400">Arquivo inválido (formato, tamanho) ou faltando.</response>
    /// <response code="422">Produto não encontrado.</response>
    [HttpPost("foto")]
    [ProducesResponseType(typeof(EnviarFotoProdutoMinioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IResult> UploadImage(
        [FromForm] EnviarFotoProdutoMinioCommand command)
    {
        var result = await mediator.Send(command);
        return resultSerializer.Serialize(result);
    }
}