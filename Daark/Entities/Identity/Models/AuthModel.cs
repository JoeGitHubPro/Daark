using System.Text.Json.Serialization;

namespace Daark.Entities.Identity.Models
{
    public class AuthModel
    {
  
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [JsonIgnore]
        public string? Id { get; set; }
        public string? Message { get; set; }
        [JsonIgnore]
        public bool IsAuthenticated { get; set; }
        public string Team { get; set; }
        public int UserId { get; set; }
      
        [JsonIgnore]
        public string? Username { get; set; }
       
        [JsonIgnore]
        public string? Email { get; set; }
       
        [JsonIgnore]
        public List<string>? Roles { get; set; }
        public string? Token { get; set; }
       
        [JsonIgnore]
        public DateTime? ExpiresOn { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }
        [JsonIgnore]
        public DateTime RefreshTokenExpiration { get; set; }
    }

 
}
