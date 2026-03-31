namespace OnboardingCreateAccount.Application.DTOs;

public record AccountResponse(Guid Id, string OwnerName, string Document, bool IsActive);
