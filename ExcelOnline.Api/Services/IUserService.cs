using ExcelOnline.Api.Options;
using ExcelOnline.Data.Models;

namespace ExcelOnline.Api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsers(UserQueryOption option);
        Task<User?> GetUser(int id);

        Task<User?> GetUserByPhone(string phoneNumber);

        Task AddUser(User input);

        Task<bool> ValidateUser(UserQueryOption option);
    }
}
