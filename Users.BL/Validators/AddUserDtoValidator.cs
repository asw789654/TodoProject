using FluentValidation;
using Users.BL.DTO;

namespace Users.BL.Validators
{
    public class AddUserDtoValidator : AbstractValidator<AddUserDto>
    {
        public AddUserDtoValidator() 
        {
            RuleFor(e => e.Name).MinimumLength(2).MaximumLength(20).WithMessage("Name error");
        }
    }
}
