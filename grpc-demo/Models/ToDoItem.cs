using System.ComponentModel.DataAnnotations;

namespace grpc_demo.Models
{
    public class ToDoItem
    {
        [Key]
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string ToDoStatus { get; set; } = "NEW";
    }
}
