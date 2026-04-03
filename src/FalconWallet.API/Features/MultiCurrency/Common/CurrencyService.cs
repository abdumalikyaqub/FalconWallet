using FalconWallet.API.Features.MultiCurrency.Common.Interfaces;
using FalconWallet.API.Features.MultiCurrency.Repositories.Interfaces;

namespace FalconWallet.API.Features.MultiCurrency.Common;

public class CurrencyService(ICurrencyRepository currencyRepository) : ICurrencyService
{
    public async Task<Currency> CreateAsync(string code, string name, decimal conversionRate,
        CancellationToken cancellationToken = default)
    {
        if (await currencyRepository.ExistsByCodeAsync(code))
            throw new CurrencyAlreadyExistException(code);

        if (conversionRate == 0)
            throw new InvalidConversionRateException();

        var newCurrency = Currency.Create(name, code, conversionRate);

        await currencyRepository.AddAsync(newCurrency);
        await currencyRepository.SaveChangesAsync();

        return newCurrency;
    }

    public async Task UpdateConversionRateAsync(int currencyId,
        decimal conversionRate,
        CancellationToken cancellationToken = default)
    {
        if (conversionRate == 0)
            throw new InvalidConversionRateException();

        Currency? currency =
            await currencyRepository.GetByIdAsync(currencyId);

        if (currency is null)
            throw new CurrencyNotFoundException(currencyId);

        currency.UpdateConversionRate(conversionRate);
        await currencyRepository.SaveChangesAsync();
    }

    public async Task<bool> HasByIdAsync(int currencyId, CancellationToken cancellationToken = default)
    {
        return await currencyRepository.HasByIdAsync(currencyId);
    }
}