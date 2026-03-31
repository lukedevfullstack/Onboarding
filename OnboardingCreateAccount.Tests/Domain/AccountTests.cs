using OnboardingCreateAccount.Domain.Entities;
using FluentAssertions;

namespace OnboardingCreateAccount.UnitTests.Domain;

public class AccountTests
{
    [Fact]
    public void NewAccount_ShouldBeActiveByDefault()
    {
        // Act
        var account = new Account("Lucas", "12345678901");

        // Assert
        account.IsActive.Should().BeTrue();
        account.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Update_ShouldChangeAccountData()
    {
        // Arrange
        var account = new Account("Antigo", "111");

        // Act
        account.Update("Novo Nome", "222", false);

        // Assert
        account.OwnerName.Should().Be("Novo Nome");
        account.IsActive.Should().BeFalse();
    }
}