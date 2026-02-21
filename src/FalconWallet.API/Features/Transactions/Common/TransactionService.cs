using FalconWallet.API.Common.Persistence;
using FalconWallet.API.Features.UserWallet.Common;
using Microsoft.EntityFrameworkCore;

namespace FalconWallet.API.Features.Transactions.Common;

internal class TransactionService(
    WalletService walletService,
    WalletDbContext walletDbContext)
{
    public async Task DepositAsync(Guid walletId,
        decimal amount,
        string? description,
        CancellationToken cancellationToken)
    {
        await ValidateTransactionAsync(walletId, amount, cancellationToken);

        var dbTransaction = await walletDbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await walletService.DepositAsync(walletId, amount, cancellationToken);

            Transaction depositTransaction = Transaction.CreateDepositTransaction(walletId,
                amount,
                description);

            await walletDbContext.Transactions.AddAsync(depositTransaction,
                cancellationToken);

            await walletDbContext.SaveChangesAsync(cancellationToken);

            await dbTransaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await dbTransaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    internal async Task WithdrawAsync(Guid walletId,
        decimal amount,
        string? description,
        CancellationToken cancellationToken)
    {
        await ValidateTransactionAsync(walletId, amount, cancellationToken);

        var dbTransaction = await walletDbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await walletService.WithdrawAsync(walletId, amount, cancellationToken);

            Transaction withdrawTransaction = Transaction.CreateWithdrawTransaction(walletId,
                amount,
                description);

            await walletDbContext.Transactions.AddAsync(withdrawTransaction,
                cancellationToken);

            await walletDbContext.SaveChangesAsync(cancellationToken);

            await dbTransaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await dbTransaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task ValidateTransactionAsync(Guid walletId,
        decimal amount,
        CancellationToken cancellationToken)
    {
        if (!await walletService.IsWalletAvailable(walletId, cancellationToken))
        {
            throw new WalletNotAvailableException(walletId);
        }

        if (amount == 0)
        {
            throw new InvalidAmountException();
        }
    }

    public async Task<List<Transaction>> GetTransactionsForWalletAsync(Guid walletId,
        CancellationToken cancellationToken)
    {
        if (!await walletService.IsWalletAvailable(walletId, cancellationToken))
        {
            throw new WalletNotAvailableException(walletId);
        }

        return await walletDbContext.Transactions.Where(x => x.WalletId == walletId)
            .OrderByDescending(x => x.CreatedOn)
            .ToListAsync(cancellationToken);
    }
}