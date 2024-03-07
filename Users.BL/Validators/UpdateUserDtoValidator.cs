using FluentValidation;
using Users.BL.DTO;

namespace Users.BL.Validators
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(e => e.Id).GreaterThan(0).WithMessage("Id error");
            RuleFor(e => e.Name).MinimumLength(2).MaximumLength(20).WithMessage("Name error");
        }
    }
}
