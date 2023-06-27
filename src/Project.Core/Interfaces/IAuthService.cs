using Project.Core.Options;
using Project.Core.Options.Params.CreateUpdate;

namespace Project.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResp> Login(string email, string password);
        Task<AuthResp> Registr(UserReg userReg);
        Task<string> Refresh(string accessToken, string refreshToken);
    }
}
