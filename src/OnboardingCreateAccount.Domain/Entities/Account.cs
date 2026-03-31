namespace OnboardingCreateAccount.Domain.Entities;

public class Account
{
    public Guid Id { get; private set; }
    public string OwnerName { get; private set; }
    public string Document { get; private set; }
    public bool IsActive { get; private set; }

    public Account(string ownerName, string document)
    {
        Id = Guid.NewGuid();
        Update(ownerName, document, true);
    }

    public void Update(string name, string doc, bool active)
    {
        OwnerName = name;
        Document = doc;
        IsActive = active;
    }

    public void Deactivate() => IsActive = false;
}
