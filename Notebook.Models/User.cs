﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [StringLength(50, MinimumLength = 3)]
        public string Password { get; set; }

		[Required]
        [StringLength(20, MinimumLength = 3)]
        public string Role { get; set; } = "user";

        public List<Note> Notes { get; set; }
    }
}
