using AutoMapper;
using Finance.Business.Models;
using Finance.Data;
using Finance.Data.Models;
using System;
using System.Threading.Tasks;

namespace Finance.Business.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly FinApiDbContext _context;
        private readonly IMapper _mapper;

        public UserService(FinApiDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserModel> GetUserAsync(string loginProvider, string loginIdentifier)
        {
            if (string.IsNullOrWhiteSpace(loginProvider))
            {
                throw new ArgumentException("The value must not be null, empty or whitespace string.", nameof(loginProvider));
            }
            if (string.IsNullOrWhiteSpace(loginIdentifier))
            {
                throw new ArgumentException("The value must not be null, empty or whitespace string.", nameof(loginIdentifier));
            }

            var login = await GetUserLogin(loginProvider, loginIdentifier);

            return _mapper.Map<UserModel>(login?.User);
        }

        public async Task<UserModel> CreateUserAsync(string name, string loginProvider, string loginIdentifier)
        {
            var login = await GetUserLogin(loginProvider, loginIdentifier);
            if (login != null)
            {
                throw new InvalidOperationException("Login with given Provider and Identifier already exists.");
            }

            var userToSave = new User
            {
                Name = name,
                UserLogins = new[]
                {
                    new UserLogin
                    {
                        Provider = loginProvider,
                        Identifier = loginIdentifier
                    }
                }
            };

            _context.Users.Add(userToSave);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserModel>(userToSave);
        }

        private async Task<UserLogin> GetUserLogin(string loginProvider, string loginIdentifier)
        {
            return await _context.UserLogins.FindAsync(loginProvider, loginIdentifier);
        }
    }
}
