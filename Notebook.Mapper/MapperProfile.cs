using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Notebook.Common.Dto;
using Notebook.Models;

namespace Notebook.Mapper
{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{ 
			CreateMap<UserCreateDto, User>();
            CreateMap<UserLogInDto, User>();
            CreateMap<NoteCreateDto, Note>();
            //CreateMap<NoteCreateDto, Note>().ForMember(d => d.Photo, opt => opt.MapFrom(s =>
            //{
            //    MemoryStream target = new MemoryStream();
            //    model.File.InputStream.CopyTo(target);
            //    return target.ToArray();
            //}));
        }
    }
}
