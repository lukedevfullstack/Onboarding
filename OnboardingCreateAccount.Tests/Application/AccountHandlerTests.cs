using FluentAssertions;
using MediatR;
using Moq;
using OnboardingCreateAccount.Application.Commands;
using OnboardingCreateAccount.Application.DTOs;
using OnboardingCreateAccount.Application.Handler;
using OnboardingCreateAccount.Application.Queries;
using OnboardingCreateAccount.Domain.Entities;
using OnboardingCreateAccount.Domain.Events;
using OnboardingCreateAccount.Domain.Interfaces;

namespace OnboardingCreateAccount.UnitTests.Application;

public class AccountHandlerTests
{
    private readonly Mock<IAccountRepository> _repositoryMock;
    private readonly Mock<ICacheService> _cacheMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AccountHandler _handler;

    public AccountHandlerTests()
    {
        _repositoryMock = new Mock<IAccountRepository>();
        _cacheMock = new Mock<ICacheService>();
        _mediatorMock = new Mock<IMediator>();

        _handler = new AccountHandler(
            _repositoryMock.Object,
            _cacheMock.Object,
            _mediatorMock.Object);
    }

    [Fact]
    public async Task Create_ShouldThrowException_WhenCpfAlreadyExists()
    {
        // Arrange
        var command = new CreateAccountCommand { OwnerName = "Lucas", Document = "12345678901" };
        _repositoryMock.Setup(r => r.ExistsByDocumentAsync(command.Document))
            .ReturnsAsync(true);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Já existe uma conta cadastrada com este CPF.");
    }

    [Fact]
    public async Task Create_ShouldSaveAndPublishEvent_WhenDataIsValid()
    {
        // Arrange
        var command = new CreateAccountCommand { OwnerName = "Lucas", Document = "12345678901" };
        _repositoryMock.Setup(r => r.ExistsByDocumentAsync(command.Document)).ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.OwnerName.Should().Be(command.OwnerName);

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Account>()), Times.Once);

        _mediatorMock.Verify(m => m.Publish(It.IsAny<AccountCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_ShouldReturnFromCache_WhenCacheExists()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var query = new GetAccountByIdQuery(accountId);
        var cachedResponse = new AccountResponse(accountId, "Lucas", "123", true);

        _cacheMock.Setup(c => c.GetAsync<AccountResponse>(It.IsAny<string>()))
            .ReturnsAsync(cachedResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(cachedResponse);
        _repositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
    }
}