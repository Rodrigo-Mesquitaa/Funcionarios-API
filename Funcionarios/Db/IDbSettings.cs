namespace Funcionarios.Db
{
    public interface IDbSettings
    {
        string FuncionariosCollectionName { get; set; }
        string TodosCollectionName { get; set; }

        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
