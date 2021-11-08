using AutoMapper;
using Finance.Business.Models;
using Finance.Data;
using Finance.Data.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.Business.Services.Implementation
{
    public class OperationService : IOperationService
    {
        private readonly FinApiDbContext _context;
        private readonly IMapper _mapper;

        public OperationService(FinApiDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OperationModel> CreateOperation(int authorId, int accountId, int categoryId, double ammount, DateTime? madeAt = null)
        {
            if (!madeAt.HasValue)
            {
                madeAt = DateTime.UtcNow;
            }

            var operation = new Operation
            {
                AccountId = accountId,
                Ammount = ammount,
                AuthorId = authorId,
                CategoryId = categoryId,
                CreatedOn = madeAt.Value
            };
            
            await _context.Operations.AddAsync(operation);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            when (ex?.InnerException is MySqlException mySqlEx && mySqlEx.ErrorCode == MySqlErrorCode.NoReferencedRow2)
            {
                throw new ArgumentException("Account with given Id does not exist.", nameof(accountId));
            }

            return _mapper.Map<OperationModel>(operation);

        }

        public IQueryable<OperationModel> QueryOperations(int accountId)
        {
            var query = _context.Operations.Where(x => x.AccountId == accountId);

            return _mapper.ProjectTo<OperationModel>(query);
        }
    }
}
