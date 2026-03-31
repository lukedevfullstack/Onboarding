using FluentValidation;
using OnboardingCreateAccount.Application.Commands;

namespace OnboardingCreateAccount.Application.Validators;

public class UpdateAccountValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O ID da conta é obrigatório para atualização.");

        RuleFor(x => x.OwnerName)
            .NotEmpty().WithMessage("O nome não pode ser vazio.")
            .MaximumLength(150);

        RuleFor(x => x.Document)
            .NotEmpty()
            .Matches(@"^\d{11}$|^\d{3}\.\d{3}\.\d{3}-\d{2}$");
    }
}