using FalconWallet.API.Common.Persistence;
using FalconWallet.API.Features.MultiCurrency.Common;
using FalconWallet.API.Features.MultiCurrency.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FalconWallet.API.Features.MultiCurrency.Repositories
{
    public class CurrencyRepository(WalletDbContext dbContext) : ICurrencyRepository
    {
        public async Task<bool> ExistsByCodeAsync(string code)
        {
            return await dbContext.Currencies.AnyAsync(c => c.Code == code);
        }

        public async Task AddAsync(Currency currency)
        {
            await dbContext.Currencies.AddAsync(currency);
        }

        public async Task<Currency?> GetByIdAsync(int id)
        {
            return await dbContext.Currencies.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> HasByIdAsync(int id)
        {
            return await dbContext.Currencies.AnyAsync(c => c.Id == id);
        }
    }
}