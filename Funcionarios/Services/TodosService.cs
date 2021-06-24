using Funcionarios.Db;
using Funcionarios.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Funcionarios.Services
{
    public class TodosService
    {
        private readonly IMongoCollection<TodoDTO> todos;
        private readonly ILogger<TodosService> logger;

        public TodosService(IDbSettings dbSettings,
            ILogger<TodosService> logger)
        {
            this.logger = logger;
            var client = new MongoClient(dbSettings.ConnectionString);
            var db = client.GetDatabase(dbSettings.DatabaseName);
            todos = db.GetCollection<TodoDTO>(dbSettings.TodosCollectionName);
        }

        public Task<List<TodoDTO>> GetFuncionarioTodos(string funcionarioId)
        {
            List<TodoDTO> allTodos = null;
            try
            {
                var result = todos.Find(todo => todo.FuncionarioId.Equals(funcionarioId))
                    .Sort("{Priority: 1}");
                allTodos = result.ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Todos falharam.");
            }
            return Task.FromResult(allTodos);
        }


        internal async Task<bool> CreateTodo(TodoDTO newTodo)
        {
            var isCreated = false;
            try
            {
                await todos.InsertOneAsync(newTodo);
                isCreated = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Falha ao criar registro de tarefas.");
            }
            return isCreated;
        }

        internal async Task<TodoDTO> GetTodoById(string id)
        {
            TodoDTO foundTodo = null;
            try
            {
                var result = await todos.FindAsync(todo => ObjectId.Parse(id).Equals(todo.Id));
                foundTodo = result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Falha ao obter tarefas por ID.");
            }
            return foundTodo;
        }

        internal async Task<bool> UpdateTodo(string id, TodoDTO updatedTodoDTO)
        {
            var isUpdated = false;
            try
            {
                var foundTodo = GetTodoById(id).Result;

                foundTodo.Title = updatedTodoDTO.Title;
                foundTodo.Description = updatedTodoDTO.Description;
                foundTodo.Priority = updatedTodoDTO.Priority;
                foundTodo.State = updatedTodoDTO.State;
                foundTodo.Estimate = updatedTodoDTO.Estimate;

                await todos.ReplaceOneAsync(todo => ObjectId.Parse(id).Equals(todo.Id), foundTodo);
                isUpdated = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Falha ao atulizar tarefa.");
            }
            return isUpdated;
        }

        internal async Task<bool> DeleteTodo(string id)
        {
            var isDeleted = false;
            try
            {
                await todos.DeleteOneAsync(todo => ObjectId.Parse(id).Equals(todo.Id));
                isDeleted = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Falha ao deletar tarefa.");
            }
            return isDeleted;
        }

        internal async Task<bool> DeleteFuncionarioTodos(string funcionarioId)
        {
            var isDeleted = false;
            try
            { 
                await todos.DeleteManyAsync(todo => todo.FuncionarioId.Equals(funcionarioId));
                isDeleted = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Falha ao excluir tarefas do funcionário.");
            }
            return isDeleted;
        }
    }
}
