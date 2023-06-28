using Notebook.Common.Dto;
using Notebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notebook.BusinessLogic.Interfaces
{
	public interface IUserService
	{
		void Create(UserCreateDto userCreateDto);

		List<User> GetAll();

		User Get(UserLogInDto userLogInDto);

		User GetUserById(int id);

		void DeleteUserById(int id);
	}
}
