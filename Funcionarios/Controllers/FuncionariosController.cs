using AutoMapper;
using Funcionarios.Models;
using Funcionarios.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Funcionarios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FuncionariosController : ControllerBase
    {
        private readonly FuncionarioService funcionarioService;
        private readonly ILogger<FuncionariosController> logger;
        private readonly IMapper mapper;

        public FuncionariosController(FuncionarioService funcionarioService,
            IMapper mapper,
            ILogger<FuncionariosController> logger)
        { 
            this.funcionarioService = funcionarioService;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<IEnumerable<FuncionarioViewModel>> GetAllFuncionarios()
        {
            var funcionarios = await funcionarioService.GetAllFuncionarios();
            var result = mapper.Map<List<FuncionaroDTO>, IEnumerable<FuncionarioViewModel>>(funcionarios);
            return result;
        } 
        
        // GET: api/Employees/id
        [HttpGet("{id}", Name = "GetFuncionarioById")]  
        public async Task<IActionResult> GetFuncionarioById(string id)
        {
            var foundFuncionario = await funcionarioService.GetFuncionarioById(id);
            if (foundFuncionario == null)
            { 
                return BadRequest();
            }
            return Ok(mapper.Map<FuncionarioViewModel>(foundFuncionario));
        }
        
        // POST: api/Employees
        [HttpPost]
        public async Task<IActionResult> CreateFuncionarioRecord([FromBody] FuncionaroDTO newFuncionarioDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(); 
            }
            var isCreated = await funcionarioService.CreateFuncionarioRecord(newFuncionarioDetails);

            if (isCreated == false)
            {
                return BadRequest(); 
            }
            return Ok();
        }
        
        // PUT: api/Employees/id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(string id, [FromBody] FuncionaroDTO funcionario)
        {
            var isUpdated = await funcionarioService.UpdateFuncionarioRecord(id, funcionario);
            if(isUpdated == false)
            {
                return BadRequest();
            }
            return Ok();
        }

        // DELETE: api/ApiWithActions/id
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(string id)
        {
            var isDeleted = await funcionarioService.DeleteFuncionarioRecord(id);
            if(isDeleted == false)
            {
                return BadRequest();
            } 
            return Ok();
        }

        public class FuncionarioViewModel
        {
        }
    }
}
