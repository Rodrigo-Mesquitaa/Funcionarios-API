using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Funcionarios.Models
{
    public class TodoDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [Required]
        [StringLength(100)]
        public string EmployeeId { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public PriorityStatus Priority { get; set; }

        [Required]
        public States State { get; set; }

        public DateTime Estimate { get; set; }
        public object FuncionarioId { get; internal set; }
    }
}

