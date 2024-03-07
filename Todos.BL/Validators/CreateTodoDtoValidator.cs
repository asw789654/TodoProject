using FluentValidation;
using Todos.BL.DTO;

namespace Todos.BL.Validators
{
    public class CreateTodoDtoValidator : AbstractValidator<CreateTodoDto>
    {
        public CreateTodoDtoValidator()
        {
            RuleFor(e => e.Id).GreaterThan(0).WithMessage("Id error");
            RuleFor(e => e.OwnerId).GreaterThan(0).WithMessage("OwnerId error");
            RuleFor(e => e.Label).MinimumLength(2).MaximumLength(20).WithMessage("Label error");
        }
    }
}
