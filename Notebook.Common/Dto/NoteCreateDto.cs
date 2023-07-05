using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notebook.Common.Dto
{
    public class NoteCreateDto
    {
        public int UserId { get; set; }

        public DateTime Date { get; set; } = DateTime.Now.Date;

        public string Name { get; set; }

        public string? Description { get; set; } = null;

        //public List<string>? Categories { get; set; }

        public byte[]? Photo { get; set; } = null;

        public string GetInfo() { return $"NOTE: {Date}, {Name}, {Description}"; }
    }
}
