using FluentValidation;

namespace Application.Users.Commands.Create
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator() 
        {
            RuleFor(u => u.Name).NotEmpty();    
        }
    }
}