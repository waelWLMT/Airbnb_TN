using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UseCases.Commands;
using FluentValidation;

namespace Application.UseCases.Validators
{
    public class UserCreateCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public UserCreateCommandValidator()
        {
            RuleFor(x=> x.UserCreateDto)
                .NotNull()
                .WithMessage("Les données de création de l'utilisateur sont obligatoires.");
        }
    }
}
