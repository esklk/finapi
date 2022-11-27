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
    public class OperationCategoryService : IOperationCategoryService
    {
        private readonly FinApiDbContext _context;
        private readonly IMapper _mapper;

        public OperationCategoryService(FinApiDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OperationCategoryModel[]> GetCategoriesAsync(int accountId)
        {
            IQueryable<OperationCategory> query = _context.OperationCategories.Where(x => x.AccountId == accountId);

            return await _mapper.ProjectTo<OperationCategoryModel>(query).ToArrayAsync();
        }

        public async Task<OperationCategoryModel> CreateCategoryAsync(string name, bool isIncome, int accountId)
        {
            var category = new OperationCategory
            {
                Name = name,
                IsIncome = isIncome,
                AccountId = accountId
            };
            await _context.OperationCategories.AddAsync(category);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            //when (ex?.InnerException is MySqlException mySqlEx && mySqlEx.ErrorCode == MySqlErrorCode.NoReferencedRow2)
            {
                throw new ArgumentException("Account with given Id not found.", nameof(accountId));
            }

            return _mapper.Map<OperationCategoryModel>(category);
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            OperationCategory category = await _context.OperationCategories.FindAsync(categoryId);
            if(category == null)
            {
                throw new ArgumentException("Operation Category with given Id does not exist.", nameof(categoryId));
            }

            _context.Remove(category);

            await _context.SaveChangesAsync();
        }
    }
}
