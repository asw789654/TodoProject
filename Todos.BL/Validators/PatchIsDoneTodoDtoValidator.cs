using FluentValidation;
using Todos.BL.DTO;

namespace Todos.BL.Validators
{
    public class PatchIsDoneTodoDtoValidator : AbstractValidator<PatchIsDoneTodoDto>
    {
        public PatchIsDoneTodoDtoValidator()
        {
            RuleFor(e => e.Id).GreaterThan(0).WithMessage("Id error");
        }
    }
}
