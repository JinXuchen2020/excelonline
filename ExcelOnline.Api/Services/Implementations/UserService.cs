using ExcelOnline.Api.Options;
using ExcelOnline.Data.Models;
using ExcelOnline.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExcelOnline.Api.Services.Implementations
{
    public class UserService : IUserService
    {
        protected readonly IRepository repository;

        protected readonly IHttpContextAccessor httpContextAccessor;

        public IRepository Repository { get => this.repository; }

        public UserService(IRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            this.repository = repository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<User>> GetUsers(UserQueryOption option)
        {
            var query = repository.GetQuery<User>().AsNoTracking();

            if (!string.IsNullOrEmpty(option.Name) && !string.IsNullOrEmpty(option.PhoneNumber))
            {
                query = query.Where(c => c.Name.Contains(option.Name) || c.PhoneNumber.Contains(option.PhoneNumber));
            }

            option.Total = query.Count();

            if (option.PageSize.HasValue)
            {
                query = query.Skip(((option.PageNo ?? 1) - 1) * option.PageSize.Value).Take(option.PageSize.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<User?> GetUser(int id)
        {
            return await this.repository.GetQuery<User>().Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByPhone(string phoneNumber)
        {
            return await this.repository.GetQuery<User>().Where(c => c.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
        }

        public async Task AddUser(User input)
        {
            await repository.AddAsync(input);
        }

        public async Task<bool> ValidateUser(UserQueryOption option) 
        {
            var result = this.repository.GetQuery<User>();
            if (!string.IsNullOrEmpty(option.PhoneNumber) && string.IsNullOrEmpty(option.Name))
            {
                result = result.Where(c => c.PhoneNumber == option.PhoneNumber);
            }
            else if (string.IsNullOrEmpty(option.PhoneNumber) && !string.IsNullOrEmpty(option.Name))
            {
                result = result.Where(c => c.Name == option.Name);
            }
            else 
            {
                result = result.Where(c => c.Name == option.Name || c.PhoneNumber == option.PhoneNumber);
            }

            return (await result.FirstOrDefaultAsync()) != null; 
        }
    }
}
