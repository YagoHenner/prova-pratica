using Application.Services.ErrorHandling;
using FluentResults;
using FluentValidation;
using MediatR;

namespace Application.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : ResultBase, new()
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var validationContext = new ValidationContext<TRequest>(request);

        var validationTasks = _validators.Select(v => v.ValidateAsync(validationContext, cancellationToken));
        var validationResults = await Task.WhenAll(validationTasks);

        var errorsReasons = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                eg => eg.Key,
                eg => eg.Select(e => e.ErrorMessage).ToArray());

        if (errorsReasons.Count > 0)
        {
            var validationError = new ValidationError(errorsReasons);
            var result = new Result().WithError(validationError);
            return result.To<TResponse>();
        }

        return await next();
    }
}