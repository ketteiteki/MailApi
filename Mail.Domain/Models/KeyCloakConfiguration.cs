namespace Mail.Domain.Models;

public class KeyCloakConfiguration
{
    public string ServerRealm { get; set; } = null!;
    
    public string Metadata { get; set; } = null!;
    
    public string ClientId { get; set; } = null!;
    
    public string ClientSecret { get; set; } = null!;
    
    public string Audience { get; set; } = null!;

    public string TokenUrl { get; set; } = null!;
}