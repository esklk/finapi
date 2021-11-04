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
    public class AccountService : IAccountService
    {
        private readonly FinApiDbContext _context;
        private readonly IMapper _mapper;

        public AccountService(FinApiDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<AccountModel> CreateAccountAsync(string name, int userId)
        {
            User user = await _context.Users.FindAsync(userId);
            if(user == null)
            {
                throw new ArgumentException("User with given Id not found.", nameof(userId));
            }

            var account = new Account { Name = name, Users = new[] { user } };
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();

            return _mapper.Map<AccountModel>(account);
        }

        public async Task DeleteAccountAsync(int id)
        {
            _context.Remove(new Account { Id = id });


            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsAccountOwnedByUser(int accountId, int userId)
        {
            return await _context.Accounts
                .Where(x => x.Id == accountId)
                .SelectMany(x => x.Users)
                .AnyAsync(x => x.Id == userId);
        }
    }
}
