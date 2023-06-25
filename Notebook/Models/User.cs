using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Notebook.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
		public string Email { get; set; }

        [Required]
		[StringLength(30, MinimumLength = 3)]
		public string Login { get; set; }

		[Required]
        [DataType(DataType.Password)]
		public string Password { get; set; }

        [Required]
        public string Role { get; set; } = "user";
    }
}
