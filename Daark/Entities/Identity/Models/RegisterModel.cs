using System.ComponentModel.DataAnnotations;

namespace Daark.Entities.Identity.Models
{
    public class RegisterModel
    {
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
        //public int UserId { get; set; }

        [StringLength(100)]
        public string Team { get; set; }

        //[StringLength(50)]
        //public string Username { get; set; }

        //[StringLength(128)]
        //public string Email { get; set; }

        [StringLength(256)]
        public string Password { get; set; }
    }
}