using Microsoft.AspNetCore.Identity;

namespace QLCHTHUOC.Services.Interfaces
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);

    }
}
