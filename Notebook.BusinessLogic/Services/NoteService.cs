using AutoMapper;
using Notebook.BusinessLogic.Interfaces;
using Notebook.Common.Dto;
using Notebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notebook.BusinessLogic.Services
{
    public class NoteService : INoteService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IMapper _mapper;

        public NoteService(ApplicationContext applicationContext, IMapper mapper)
        {
            _applicationContext = applicationContext;
            _mapper = mapper;
        }

        public void Create(NoteCreateDto noteCreateDto) 
        {
            var note = _mapper.Map<NoteCreateDto, Note>(noteCreateDto);

            _applicationContext.Notes.Add(note);
            _applicationContext.SaveChanges();
        }
    }
}
