using FluentValidation;
using FluentValidation.Results;

namespace ToDoBackend.Extensions;

public static class ValidationResultExtensions
{
    public static void CheckValidation(this ValidationResult validationResult)
    {
        if (validationResult.IsValid == false)
        {
            foreach (ValidationFailure? failure in validationResult.Errors)
                Console.WriteLine($"Property {failure.PropertyName} failed validation. Error was: {failure.ErrorMessage}");

            throw new ValidationException(validationResult.Errors);
        }
    }
}