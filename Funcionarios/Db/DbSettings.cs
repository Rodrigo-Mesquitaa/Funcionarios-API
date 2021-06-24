namespace Funcionarios.Db
{
    public class DbSettings : IDbSettings
    {
        public string FuncionariosCollectionName { get; set; }
        public string TodosCollectionName { get; set; }

        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
