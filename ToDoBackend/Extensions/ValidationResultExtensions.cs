using FluentValidation;
using FluentValidation.Results;
using Serilog;

namespace ToDoBackend.Extensions;

public static class ValidationResultExtensions 
{ 
    public static bool TryCheckValidation(this ValidationResult validationResult)
    {
        ArgumentNullException.ThrowIfNull(validationResult);

        if (validationResult.IsValid)
            return true;

        foreach (ValidationFailure failure in validationResult.Errors)
            Log.Error($"Property {failure.PropertyName} failed validation. Error was: {failure.ErrorMessage}");

        return false;
    }
}