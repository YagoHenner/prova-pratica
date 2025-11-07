using FluentResults;

namespace Application.Services.ErrorHandling;

public class UnprocessableEntityError(string message) : Error(message);