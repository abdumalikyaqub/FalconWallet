using FalconWallet.API.Features.Transactions.SendToWallet;
namespace FalconWallet.API.Features.Transactions.Common.Interfaces;

public interface ITransactionService
{
    Task DepositAsync(Guid walletId,
        decimal amount,
        string? description,
        CancellationToken cancellationToken);

      Task<List<Transaction>> GetTransactionsForWalletAsync(Guid walletId,
          CancellationToken cancellationToken);

      Task<SendToWalletResponse> SendToWalletAsync(
          Guid fromWalletId,
          Guid toWalletId,
          decimal amount,
          string? description,
          CancellationToken cancellationToken);
}