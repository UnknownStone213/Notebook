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

        public List<Note> GetAll()
        {
            var notes = _applicationContext.Notes.ToList();

            return notes;
        }

        public List<Note> GetNotesByUserId(int id)
        {
            List<Note> notes = _applicationContext.Notes.Where(u => u.UserId == id).ToList();
            return notes;
        }

        public Note GetById(int id) 
        {
            Note note = _applicationContext.Notes.FirstOrDefault(u => u.Id == id);

            return note;
        }

        public void DeleteNoteById(int id)
        {
            var note = _applicationContext.Notes.FirstOrDefault(u =>
                u.Id == id);

            _applicationContext.Notes.Remove(note);
            _applicationContext.SaveChanges();
        }

        public void Edit(Note note)
        {
            _applicationContext.Notes.Update(note);
            _applicationContext.SaveChanges();
        }
    }
}
