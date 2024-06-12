using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;
namespace QLCHTHUOC.Model.DTO
{
    public class LoginResponseDTO
    {
        public string JwtToken { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
