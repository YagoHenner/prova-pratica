using Application.Services.ErrorHandling;
using FluentResults;

namespace WebApi.Common.HttpResults;

/// <summary>
/// Serializa um objeto FluentResults.Result em um IResult padrão do ASP.NET Core.
/// </summary>
public class ResultSerializer
{

    public IResult Serialize<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return HandleError(result);
    }

    /// <summary>
    /// Serializa um resultado sem valor (ex: Update, Delete).
    /// </summary>
    public IResult Serialize(Result result)
    {
        if (result.IsSuccess)
        {
            return Results.NoContent();
        }

        return HandleError(result);
    }

    /// <summary>
    /// Converte um Result falho em um IResult (BadRequest ou UnprocessableEntity).
    /// </summary>
    private IResult HandleError(ResultBase result)
    {
        var erro = result.Errors.FirstOrDefault();
        if (erro == null)
        {
            return Results.InternalServerError(new ErrorResponse("ErroDesconhecido", ["Ocorreu um erro desconhecido."], 500));
        }

        var respostaErro = new ErrorResponse(
            errorType: erro.GetType().Name,
            message: erro switch
            {
                ValidationError erroValidacao => erroValidacao.Messages,
                _ => [erro.Message]
            },
            errorCode: erro switch
            {
                UnprocessableEntityError => 422,
                _ => 400
            }
        );

        return erro switch
        {
            UnprocessableEntityError => Results.UnprocessableEntity(respostaErro),
            _ => Results.BadRequest(respostaErro)
        };
    }
}