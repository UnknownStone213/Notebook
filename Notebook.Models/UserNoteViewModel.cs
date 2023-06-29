using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notebook.Models
{
    public class UserNoteViewModel
    {
        public IEnumerable<User> Users { get; set; } = new List<User>();
        public IEnumerable<Note> Notes { get; set; } = new List<Note>();
    }
}
