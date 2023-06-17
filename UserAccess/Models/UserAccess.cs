using System.ComponentModel.DataAnnotations;

namespace UserAccessAPI.Models
{
    public class UserDetail
    {
        public int Id { get; set; }
        [StringLength(50, ErrorMessage = "The Name field must be between {2} and {1} characters long.", MinimumLength = 2)]
        public string Username { get; set; }
        public accessLevel AccessLevel { get; set; }
        [Required(ErrorMessage = "The permission field is required")]
        public bool?Permission { get; set; }

        public DateTime timeCreated { get; set; }

        public enum accessLevel
        {
            Admin,
            StandardUser,
            Guest
        }
    }

   
}
