using FluentResults;

namespace Application.Services.ErrorHandling;

public class ValidationError : Error
{
    public Dictionary<string, string[]> ErrorMessageDictionary { get; }

    public List<string> Messages { get; }

    public ValidationError(Dictionary<string, string[]> errorMessageDictionary) : base()
    {
        ErrorMessageDictionary = errorMessageDictionary;

        Messages = [.. errorMessageDictionary.SelectMany(kvp => kvp.Value)];
    }
}

