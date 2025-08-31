using FluentValidation;
using ToDoBackend.Models.ToDoItem;

namespace ToDoBackend.Validators;

public class ToDoItemValidator : AbstractValidator<ToDoItem>
{
    public ToDoItemValidator()
    {
        const int MinTitleLength = 1;
        const int MaxTitleLength = 100;

        RuleFor(item => item.Id)
            .NotEmpty()
            .WithMessage("Id не может быть пустым!");

        RuleFor(item => item.Title)
            .NotNull()
            .NotEmpty()
            .WithMessage("Название задачи не может быть пустым!")
            .Must(title => string.IsNullOrWhiteSpace(title) == false)
            .WithMessage("Название не может состоять только из пробелов.")
            .Must(title => title.Value.Length >= MinTitleLength)
            .WithMessage($"Минимальное количество символов: {MinTitleLength}")
            .Must(title => title.Value.Length <= MaxTitleLength)
            .WithMessage($"Максимальное количество символов: {MaxTitleLength}");
    }
}