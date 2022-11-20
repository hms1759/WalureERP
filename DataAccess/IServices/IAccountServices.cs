using DataAccess.Dto;
using DataAccess.Model;
using DataAccess.Model.Identity;
using Shared.Dapper;
using System.Security.Claims;

namespace DataAccess.IServices
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAccountServices : IService<Staffs>
    {
        Task<RegisterDto> RegisterUser(RegisterDto model);
        Task<string> Login(string username, string? password, object? token);
        Task AddUserClaims(WalureUser user, ClaimsIdentity identity);

    }
}
