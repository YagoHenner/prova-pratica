using FluentResults;

namespace Application.Services.ErrorHandling;

public static class ResultExtensions
{
    public static TResult To<TResult>(this ResultBase resultBase) where TResult : ResultBase, new()
    {
        var val = new TResult();
        val.Reasons.AddRange(resultBase.Reasons);
        return val;
    }
}