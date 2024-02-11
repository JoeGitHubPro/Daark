using Daark.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Daark.Entities.Identity.Models
{

    public class ApplicationUser : IdentityUser
    {
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public int UserId { get; set; }


        public List<RefreshToken>? RefreshTokens { get; set; }
        public List<DaarkRealEstate>? DaarkRealEstates { get; set; }
    }
}