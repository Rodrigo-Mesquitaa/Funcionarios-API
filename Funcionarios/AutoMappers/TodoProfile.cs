using AutoMapper;
using Funcionarios.Models;
using Funcionarios.Model;

namespace Funcionarios.AutoMappers
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<TodoDTO, TodoViewModel>();

            CreateMap<TodoDTO, TodoOverviewViewModel>();
        }
    }
}
