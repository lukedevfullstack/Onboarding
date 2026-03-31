using OnboardingCreateAccount.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingCreateAccount.Tests.Domain;

public class AccountTests
{
    [Fact]
    public void Constructor_Should_Initialize_Correctly()
    {
        // Act
        var account = new Account("Bruce Wayne", "123.456.789-00");

        // Assert
        Assert.NotEqual(Guid.Empty, account.Id);
        Assert.True(account.IsActive);
        Assert.Equal("Bruce Wayne", account.OwnerName);
    }

    [Fact]
    public void Deactivate_Should_Change_Status_To_False()
    {
        var account = new Account("Test", "000");
        account.Deactivate();
        Assert.False(account.IsActive);
    }
}
