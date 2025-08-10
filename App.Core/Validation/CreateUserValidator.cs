using Common.Dtos.ResponseDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Validation
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress();
            RuleFor(x => x.Name).NotNull().NotEmpty().MinimumLength(2);
            RuleFor(x => x.Password).NotEmpty().NotNull().MinimumLength(5);
        }
    }
}
