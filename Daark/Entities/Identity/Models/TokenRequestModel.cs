using System.ComponentModel.DataAnnotations;

namespace Daark.Entities.Identity.Models
{
    public class TokenRequestModel
    {
        [EmailAddress]
        public string? Email { get; set; }

        public string? PhoneNumber  { get; set; }

        public string Password { get; set; }
    }
}