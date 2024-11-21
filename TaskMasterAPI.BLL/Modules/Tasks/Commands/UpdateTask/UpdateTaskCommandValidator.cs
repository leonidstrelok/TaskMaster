using FluentValidation;

namespace TaskMasterAPI.BLL.Modules.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty()
            .NotNull();
    }
}