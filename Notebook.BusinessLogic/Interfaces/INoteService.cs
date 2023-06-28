using Notebook.Common.Dto;
using Notebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notebook.BusinessLogic.Interfaces
{
    public interface INoteService
    {
        void Create(NoteCreateDto noteCreateDto);
    }
}
