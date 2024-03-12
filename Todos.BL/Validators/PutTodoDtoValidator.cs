using FluentValidation;
using Todos.BL.DTO;

namespace Todos.BL.Validators
{
    public class PutTodoDtoValidator : AbstractValidator<PutTodoDto>
    {
        public PutTodoDtoValidator()
        {
            RuleFor(e => e.Id).GreaterThan(0).WithMessage("Id error");
            RuleFor(e => e.OwnerId).GreaterThan(0).WithMessage("OwnerId error");
            RuleFor(e => e.Label).MinimumLength(1).MaximumLength(20).WithMessage("Label error");
        }
    }
}
