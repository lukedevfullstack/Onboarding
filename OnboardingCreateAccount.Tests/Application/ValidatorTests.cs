using FluentValidation.TestHelper;
using OnboardingCreateAccount.Application.Commands;
using OnboardingCreateAccount.Application.Validators;

namespace OnboardingCreateAccount.UnitTests.Application;

public class ValidatorTests
{
    private readonly CreateAccountValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData("Ab")]
    [InlineData("Este nome é propositalmente muito longo para testar o limite de cento e cinquenta caracteres que definimos na regra de validação do FluentValidation dentro do nosso comando de criação de conta bancária")] // > 150 chars
    public void ShouldHaveError_WhenNameIsInvalid(string name)
    {
        var command = new CreateAccountCommand { OwnerName = name, Document = "12345678901" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.OwnerName);
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("11111111111")]
    [InlineData("12345678901")]
    public void ShouldHaveError_WhenCpfIsInvalid(string cpf)
    {
        var command = new CreateAccountCommand { OwnerName = "Lucas", Document = cpf };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Document);
    }

    [Fact]
    public void ShouldNotHaveError_WhenCommandIsValid()
    {
        var command = new CreateAccountCommand
        {
            OwnerName = "Lucas Silva",
            Document = "37151000014"
        };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}