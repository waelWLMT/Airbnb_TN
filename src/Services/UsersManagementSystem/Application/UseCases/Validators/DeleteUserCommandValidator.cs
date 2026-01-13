using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UseCases.Commands;
using FluentValidation;

namespace Application.UseCases.Validators
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("User Id must not be empty.")
                .NotEqual(Guid.Empty).WithMessage("User Id must be a valid GUID.");
        }
    }
}
