using FalconWallet.API.Features.MultiCurrency.Common;

namespace FalconWallet.API.Features.MultiCurrency.Repositories.Interfaces
{
    public interface ICurrencyRepository
    {
        Task<bool> ExistsByCodeAsync(string code);
        Task AddAsync(Currency currency);
        Task<Currency?> GetByIdAsync(int id);
        Task SaveChangesAsync();
        Task<bool> HasByIdAsync(int id);
    }
}