using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notebook.Common.Dto
{
	public class UserCreateDto
	{
		public string Email { get; set; }

		public string Login { get; set; }

		public string Password { get; set; }

		public string Role { get; set; } = "user";
	}
}
