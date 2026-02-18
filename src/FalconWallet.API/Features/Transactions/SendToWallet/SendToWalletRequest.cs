namespace FalconWallet.API.Features.Transactions.SendToWallet;

public sealed record SendToWalletRequest(
    Guid FromWalletId,
    Guid ToWalletId,
    decimal Amount,
    string? Description
);