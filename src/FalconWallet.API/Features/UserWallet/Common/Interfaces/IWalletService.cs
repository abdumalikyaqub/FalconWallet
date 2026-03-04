namespace FalconWallet.API.Features.UserWallet.Common.Interfaces;

public interface IWalletService
{
    Task<Wallet> CreateAsync(Guid userId,
        string? title,
        int currencyId,
        CancellationToken cancellationToken);

    Task UpdateTitleAsync(Guid walletId,
        string? title, CancellationToken cancellationToken = default);

    Task SuspendWalletAsync(Guid walletId, CancellationToken cancellationToken);

    Task<bool> IsWalletAvailable(Guid walletId, CancellationToken cancellationToken);

    Task DepositAsync(Guid walletId, decimal amount, CancellationToken cancellationToken);

    Task WithdrawAsync(Guid walletId, decimal amount, CancellationToken cancellationToken);

    Task<Wallet> GetWalletAsync(Guid walletId, CancellationToken cancellationToken);

    Task<Wallet> GetWalletWithCurrencyFromDbAsync(Guid walletId, CancellationToken cancellationToken);
}