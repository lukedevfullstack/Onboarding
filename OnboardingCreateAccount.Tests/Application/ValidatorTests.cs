using OnboardingCreateAccount.Application.Validators;
using OnboardingCreateAccount.Application.Commands;
using FluentValidation.TestHelper;

namespace OnboardingCreateAccount.UnitTests.Application;

public class ValidatorTests
{
    private readonly CreateAccountValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData("Ab")]
    public void ShouldHaveError_WhenNameIsInvalid(string name)
    {
        var command = new CreateAccountCommand { OwnerName = name, Document = "12345678901" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.OwnerName);
    }

    [Fact]
    public void ShouldHaveError_WhenCpfIsInvalid()
    {
        var command = new CreateAccountCommand { OwnerName = "Lucas", Document = "123" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Document);
    }
}