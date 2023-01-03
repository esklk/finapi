using AutoMapper;
using Finance.Business.Models;
using Finance.Data;
using Finance.Data.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<UserModel> GetUserAsync(int id)
        {
            var user = await _context.FindAsync<User>(id);

            return _mapper.Map<UserModel>(user);
        }

        public async Task<UserModel> CreateUserAsync(UserModel user)
        {
            var userToSave = _mapper.Map<User>(user);

            _context.Users.Add(userToSave);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to create a user.", ex);
            }

            return _mapper.Map<UserModel>(userToSave);
        }
    }
}
