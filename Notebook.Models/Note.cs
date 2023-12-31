﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notebook.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now.Date;

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        //public List<string>? Categories { get; set; }

        public byte[]? Photo { get; set; }

        public string GetInfo() { return $"Id={Id}, UserId={UserId}, Date={Date}, Name={Name}, Description={Description}"; }
    }
}
