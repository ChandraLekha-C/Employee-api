using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_API.Models
{
    public class employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Employee_Id { get; set; }
        public string Employee_Name { get; set; }
        public int Age { get; set; }
        public int Department_Id { get; set; }
        public string Role { get; set; }
           
    }
}
