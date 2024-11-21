using FluentValidation;

namespace TaskMasterAPI.BLL.Modules.Tasks.Commands.CreateTask;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty()
            .NotNull();
    }
}