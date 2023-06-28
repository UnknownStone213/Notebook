using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notebook.Common.Dto
{
    public class NoteCreateDto
    {
        public DateTime Created { get; set; } = DateTime.Now;

        public string Name { get; set; }

        public string? Description { get; set; }

        //public List<string>? Categories { get; set; }

        public byte[]? Photo { get; set; }
    }
}
