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
    public class FuncionarioService
    {
        private readonly IMongoCollection<FuncionaroDTO> funcionarios;
        private readonly ILogger<FuncionarioService> logger;
        private readonly TodosService todosService;

        public FuncionarioService(IDbSettings dbSettings,
            TodosService todosService,
            ILogger<FuncionarioService> logger)
        {
            this.todosService = todosService;
            this.logger = logger;
            var client = new MongoClient(dbSettings.ConnectionString);
            var db = client.GetDatabase(dbSettings.DatabaseName);
            funcionarios = db.GetCollection<FuncionaroDTO>(dbSettings.FuncionariosCollectionName);
        }

        public Task<List<FuncionaroDTO>> GetAllFuncionarios()
        {
            List<FuncionaroDTO> allFuncionarios = null;
            try
            {
                var result = funcionarios.Find(x => true).Sort("{FirstName: 1}");
                allFuncionarios = result.ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Todos os funcionários falharam.");
            }
            return Task.FromResult(allFuncionarios);
        }

        internal async Task<bool> CreateFuncionarioRecord(FuncionaroDTO newFuncionarioDetails)
        {
            var isCreated = false;
            try
            {
                await funcionarios.InsertOneAsync(newFuncionarioDetails);
                isCreated = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Falha ao criar registro de funcionário.");
            }
            return isCreated;
        }

        internal async Task<FuncionaroDTO> GetFuncionarioById(string id)
        {
            FuncionaroDTO foundFuncionario = null;
            try
            {
                var result = await funcionarios.FindAsync(funcionario => ObjectId.Parse(id).Equals(funcionario.Id));
                foundFuncionario = result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Falha ao obter funcionário por ID.");
            }
            return foundFuncionario;
        }

        internal async Task<bool> UpdateFuncionarioRecord(string id, FuncionaroDTO updatedFuncionarioDTO)
        {
            var isUpdated = false;
            try
            {
                var foundFuncionario = GetFuncionarioById(id).Result;

                foundFuncionario.FirstName = updatedFuncionarioDTO.FirstName;
                foundFuncionario.LastName = updatedFuncionarioDTO.LastName;
                foundFuncionario.Email = updatedFuncionarioDTO.Email;
                foundFuncionario.Department = updatedFuncionarioDTO.Department;

                await funcionarios.ReplaceOneAsync(emp => ObjectId.Parse(id).Equals(emp.Id), foundFuncionario);
                isUpdated = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "O Upadte do funcionario falhou.");
            }
            return isUpdated;
        }

        internal async Task<bool> DeleteFuncionarioRecord(string id)
        {
            var isDeleted = false;
            try
            {
                await funcionarios.DeleteOneAsync(emp => ObjectId.Parse(id).Equals(emp.Id));
                await todosService.DeleteFuncionarioTodos(id);
                isDeleted = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Falha ao excluir funcionario.");
            }
            return isDeleted;
        }
    }
}
