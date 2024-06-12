namespace MVCQLCHTHUOC.Models
{
    public class LoginResponseDTO
    {
        public string JwtToken { get; set; }
        public string Username { get; set; } // Thêm thuộc tính Username
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
