using Application.UseCases.Produtos.CriarProduto;
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
}