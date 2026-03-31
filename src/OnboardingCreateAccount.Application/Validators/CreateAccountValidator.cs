using FluentValidation;
using OnboardingCreateAccount.Application.Commands;
using OnboardingCreateAccount.Domain.Utils;

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
                    .Must(CpfValidator.IsValid).WithMessage("O CPF fornecido não é válido."); ;
    }
}