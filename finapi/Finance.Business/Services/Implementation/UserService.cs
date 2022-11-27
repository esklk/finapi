using AutoMapper;
using Finance.Business.Models;
using Finance.Data;
using Finance.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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

            var query = _context.UserLogins.Where(x => x.Provider == loginProvider && x.Identifier == loginIdentifier).Select(x => x.User);

            return await _mapper.ProjectTo<UserModel>(query).SingleOrDefaultAsync();
        }

        public async Task<UserModel> CreateUserAsync(string name, string loginProvider, string loginIdentifier)
        {
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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException ex) 
            //when (ex?.InnerException is MySqlException mySqlEx && mySqlEx.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
            {
                throw new InvalidOperationException("Login with given Provider and Identifier already exists.", ex);
            }

            return _mapper.Map<UserModel>(userToSave);
        }
    }
}
