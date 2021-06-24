using Funcionarios.Models;

namespace Funcionarios.Model
{
    public class TodoOverviewViewModel
    {
        public string Id { get; set; }

        public string FuncionarioId { get; set; }

        public string Title { get; set; }

        public PriorityStatus Priority { get; set; }
       
    }
}
