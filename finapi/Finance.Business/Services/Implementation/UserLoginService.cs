using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Finance.Business.Models;
using Finance.Data;
using Finance.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Finance.Business.Services.Implementation
{
    public class UserLoginService : IUserLoginService
    {
        private readonly FinApiDbContext _context;
        private readonly IMapper _mapper;

        public UserLoginService(FinApiDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserLoginModel> CreateUserLoginAsync(int userId, string loginProvider, string loginIdentifier)
        {
            var userLoginToSave = new UserLogin
            {
                Provider = loginProvider,
                Identifier = loginIdentifier,
                UserId = userId
            };

            _context.UserLogins.Add(userLoginToSave);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to create user login.", ex);
            }

            return _mapper.Map<UserLoginModel>(userLoginToSave);
        }

        public async Task<UserLoginModel?> GetUserLoginAsync(string loginProvider, string loginIdentifier)
        {
            if (string.IsNullOrWhiteSpace(loginProvider))
            {
                throw new ArgumentException("The value must not be null, empty or whitespace string.", nameof(loginProvider));
            }
            if (string.IsNullOrWhiteSpace(loginIdentifier))
            {
                throw new ArgumentException("The value must not be null, empty or whitespace string.", nameof(loginIdentifier));
            }

            IQueryable<UserLogin> query = _context.UserLogins.Where(x => x.Provider == loginProvider && x.Identifier == loginIdentifier);

            return await _mapper.ProjectTo<UserLoginModel>(query).SingleOrDefaultAsync();
        }
    }
}
