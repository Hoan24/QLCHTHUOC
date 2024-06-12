using System.ComponentModel.DataAnnotations;

namespace MVCQLCHTHUOC.Models
{
    public class RegisterViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string[] Roles
        {
            get; set;
        }
    }

}
