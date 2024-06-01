using System.ComponentModel.DataAnnotations;

namespace QLCHTHUOC.Model.DTO
{
    public class CustomerAddDTO
    {
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
