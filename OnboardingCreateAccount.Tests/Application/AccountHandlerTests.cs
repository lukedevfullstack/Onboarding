using MediatR;
using Moq;
using OnboardingCreateAccount.Application.DTOs;
using OnboardingCreateAccount.Application.Handler;
using OnboardingCreateAccount.Application.Queries;
using OnboardingCreateAccount.Domain.Interfaces;

namespace OnboardingCreateAccount.Tests.Application;

public class AccountHandlerTests
{
    [Fact]
    public async Task Handle_GetById_Should_Return_From_Cache_And_Not_Call_Repository()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var cachedResponse = new AccountResponse(accountId, "Cached User", "123", true);

        var repoMock = new Mock<IAccountRepository>();
        var cacheMock = new Mock<ICacheService>();
        var mediatorMock = new Mock<IMediator>();

        cacheMock.Setup(x => x.GetAsync<AccountResponse>(It.IsAny<string>()))
                 .ReturnsAsync(cachedResponse);

        var handler = new AccountHandler(repoMock.Object, cacheMock.Object, mediatorMock.Object);

        // Act
        var result = await handler.Handle(new GetAccountByIdQuery(accountId), default);

        // Assert
        Assert.Equal("Cached User", result?.OwnerName);

        repoMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
    }
}