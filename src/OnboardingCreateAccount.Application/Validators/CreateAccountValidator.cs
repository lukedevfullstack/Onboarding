using FluentValidation;
using OnboardingCreateAccount.Application.Commands;

namespace OnboardingCreateAccount.Application.Validators;

public class CreateAccountValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountValidator()
    {
        RuleFor(x => x.OwnerName)
            .NotEmpty().WithMessage("O nome do titular é obrigatório.")
            .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres.")
            .MaximumLength(150).WithMessage("O nome deve ter no máximo 150 caracteres.");

        RuleFor(x => x.Document)
            .NotEmpty().WithMessage("O CPF é obrigatório.")
            .Matches(@"^\d{11}$|^\d{3}\.\d{3}\.\d{3}-\d{2}$")
            .WithMessage("O CPF deve estar em um formato válido (11 dígitos).");
    }
}