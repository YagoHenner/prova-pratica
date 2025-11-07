using Application.Services.ErrorHandling;
using FluentResults;

namespace WebApi.Common.HttpResults
{
    public class ResultSerializer
    {
        public IResult Serialize<T>(Result<T> result)
        {
            if (result.IsSuccess)
            { 
                return Results.Ok(result.Value);
            }

            var error = result.Errors.FirstOrDefault();
            var errorResponse = new ErrorResponse(
                errorType: error!.GetType().Name,
                message: error switch
                {
                    ValidationError validationError => validationError.Messages,
                    _ => [error.Message]
                },
                errorCode: error switch
                {
                    UnprocessableEntityError => 422,
                    _ => 400
                }
            );

            return error switch
            {
                UnprocessableEntityError => Results.UnprocessableEntity(errorResponse),
                _ => Results.BadRequest(errorResponse)
            };
        }
    }
}
