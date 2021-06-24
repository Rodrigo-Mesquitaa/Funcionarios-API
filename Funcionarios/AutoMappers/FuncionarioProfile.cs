using AutoMapper;
using Funcionarios.Models;
using static Funcionarios.Controllers.FuncionariosController;

namespace Funcionarios.AutoMappers
{
    public class FuncionarioProfile : Profile
    {
        public FuncionarioProfile()
        {
            CreateMap<FuncionaroDTO, FuncionarioViewModel>();
        }
    }
}
