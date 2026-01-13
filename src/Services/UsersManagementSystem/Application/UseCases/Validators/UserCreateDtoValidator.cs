using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using FluentValidation;

namespace Application.UseCases.Validators
{
    public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
    {
        public UserCreateDtoValidator()
        {
            RuleFor(x=> x.Nom).NotEmpty().WithMessage("Le nom est obligatoire.");

            RuleFor(x => x.Prenom).NotEmpty().WithMessage("Le prénom est obligatoire.");

            RuleFor(x => x.Email).NotEmpty().WithMessage("L'email est obligatoire.")

                                 .EmailAddress().WithMessage("L'email n'est pas valide.");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Le mot de passe est obligatoire.")
                                    .MinimumLength(6).WithMessage("Le mot de passe doit contenir au moins 6 caractères.")
                                    .Matches("[A-Z]").WithMessage("Le mot de passe doit contenir au moins une lettre majuscule.")
                                    .Matches("[a-z]").WithMessage("Le mot de passe doit contenir au moins une lettre minuscule.")
                                    .Matches("[0-9]").WithMessage("Le mot de passe doit contenir au moins un chiffre.")
                                    .Matches("[^a-zA-Z0-9]").WithMessage("Le mot de passe doit contenir au moins un caractère spécial.");

            RuleFor(x => x.RoleId).GreaterThan(0).WithMessage("Le rôle est obligatoire et doit être valide.");
        }
    }
}
