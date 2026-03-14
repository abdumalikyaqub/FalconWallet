namespace FalconWallet.API.Features.Transactions.SendToWallet;

public sealed record SendToWalletResponse(
    Guid FromWalletId,
    Guid ToWalletId,
    decimal Amount,
    Guid WithdrowTransactionId,
    Guid DepositTransactionId
);