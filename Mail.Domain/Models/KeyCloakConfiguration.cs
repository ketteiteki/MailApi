namespace Mail.Domain.Models;

public class KeyCloakConfiguration
{
    public required string ServerRealm { get; set; }
    
    public required string Metadata { get; set; }
    
    public required string ClientId { get; set; }
    
    public required string ClientSecret { get; set; }
    
    public required string Audience { get; set; }

    public required string TokenUrl { get; set; }
}