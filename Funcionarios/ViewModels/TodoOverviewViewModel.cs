using Funcionarios.Models;
using System;

namespace Funcionarios.Model
{
    public class TodoViewModel
    {
        public string Id { get; set; }

        public string EmployeeId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public PriorityStatus Priority { get; set; }

        public States State { get; set; }

        public DateTime Estimate { get; set; }
    }
}
