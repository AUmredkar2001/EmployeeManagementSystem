using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class Authentication
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
