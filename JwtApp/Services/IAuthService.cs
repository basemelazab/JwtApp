using JwtApp.Models;
using System.Threading.Tasks;

namespace JwtApp.Services
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
    }
}
