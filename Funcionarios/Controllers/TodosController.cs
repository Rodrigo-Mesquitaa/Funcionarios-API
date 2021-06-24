using AutoMapper;
using Funcionarios.Models;
using Funcionarios.Services;
using Funcionarios.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Funcionarios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly TodosService todosService;
        private readonly ILogger<TodosController> logger;
        private readonly IMapper mapper;

        public TodosController(TodosService TodosService,
            IMapper mapper,
            ILogger<TodosController> logger)
        {
            this.todosService = TodosService;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET: api/Todos
        [HttpGet("{funcionarioId}", Name = "GetFuncionarioTodos")]
        public async Task<IEnumerable<TodoOverviewViewModel>> GetFuncionarioTodos(string funcionarioId)
        {
            var Todos = await todosService.GetFuncionarioTodos(funcionarioId);
            var result = mapper.Map<List<TodoDTO>, IEnumerable<TodoOverviewViewModel>>(Todos);
            return result;
        }

        // GET: api/Todos/id
        [HttpGet("{funcionarioId}/{id}", Name = "GetTodoById")]
        public async Task<IActionResult> GetTodoById(string funcionarioId, string id)
        {
            var foundTodo = await todosService.GetTodoById(id);
            if (foundTodo == null)
            {
                return BadRequest();
            }
            return Ok(mapper.Map<TodoViewModel>(foundTodo));
        }

        // POST: api/Todos
        [HttpPost]
        public async Task<IActionResult> CreateTodo([FromBody] TodoDTO newTodo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var isCreated = await todosService.CreateTodo(newTodo);

            if (isCreated == false)
            {
                return BadRequest();
            }
            return Ok();
        }

        // PUT: api/Todos/id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(string id, [FromBody] TodoDTO updatedTodo)
        {
            var isUpdated = await todosService.UpdateTodo(id, updatedTodo);
            if (isUpdated == false)
            {
                return BadRequest();
            }
            return Ok();
        }

        // DELETE: api/ApiWithActions/id
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodo(string id)
        {
            var isDeleted = await todosService.DeleteTodo(id);
            if (isDeleted == false)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
