using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;
namespace QLCHTHUOC.Model.DTO
{
    public class LoginResponseDTO
    {
        public string JwtToken { set; get; }
    }
}
