using System.ComponentModel.DataAnnotations;

namespace MVCQLCHTHUOC.Models
{
    public class CustomerEditDTO
    {
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
