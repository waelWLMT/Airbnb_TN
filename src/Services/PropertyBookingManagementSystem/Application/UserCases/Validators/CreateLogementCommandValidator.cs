using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UserCases.Commands;
using FluentValidation;

namespace Application.UserCases.Validators
{
    public class CreateLogementCommandValidator : AbstractValidator<CreateLogementCommand>
    {
        public CreateLogementCommandValidator()
        {
            RuleFor(x=> x).NotNull();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.OwnerId).GreaterThan(0);
            RuleFor(x => (int)x.TypeLogement).InclusiveBetween(1, 4);
            RuleFor(x => x.PrixParNuit).GreaterThan(0);
        }
    }
}
