namespace ElegiBien.Web.Models.Contact;

public sealed class ContactViewModel
{
    public string? Email { get; init; }

    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(Email);
}
