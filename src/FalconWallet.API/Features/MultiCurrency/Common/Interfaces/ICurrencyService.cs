namespace FalconWallet.API.Features.MultiCurrency.Common.Interfaces;

public interface ICurrencyService
{
    Task<Currency> CreateAsync(string code,
        string name,
        decimal conversionRate,
        CancellationToken cancellationToken = default);

    Task UpdateConversionRateAsync(int currencyId,
        decimal conversionRate,
        CancellationToken cancellationToken = default);
    
    Task<bool> HasByIdAsync(int currencyId, CancellationToken cancellationToken = default);
}