using FalconWallet.API.Features.Transactions.SendToWallet;
using FalconWallet.API.Common.Persistence;
using FalconWallet.API.Features.UserWallet.Common;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace FalconWallet.API.Features.Transactions.Common;

internal class TransactionService(WalletService walletService,
                                  WalletDbContext walletDbContext)
{
    private readonly WalletService _walletService = walletService;
    private readonly WalletDbContext _walletDbContext = walletDbContext;

    public async Task DepositAsync(Guid walletId,
                                   decimal amount,
                                   string? description,
                                   CancellationToken cancellationToken)
    {
        await ValidateTransactionAsync(walletId, amount, cancellationToken);

        var dbTransaction = await _walletDbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await _walletService.DepositAsync(walletId, amount, cancellationToken);

            Transaction depositTransaction = Transaction.CreateDepositTransaction(walletId,
                                                                                  amount,
                                                                                  description);

            await _walletDbContext.Transactions.AddAsync(depositTransaction,
                                                        cancellationToken);

            await _walletDbContext.SaveChangesAsync(cancellationToken);

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

        var dbTransaction = await _walletDbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await _walletService.WithdrawAsync(walletId, amount, cancellationToken);

            Transaction withdrawTransaction = Transaction.CreateWithdrawTransaction(walletId,
                                                                                    amount,
                                                                                    description);

            await _walletDbContext.Transactions.AddAsync(withdrawTransaction,
                                                        cancellationToken);

            await _walletDbContext.SaveChangesAsync(cancellationToken);

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
        if (!await _walletService.IsWalletAvailable(walletId, cancellationToken))
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
        if (!await _walletService.IsWalletAvailable(walletId, cancellationToken))
        {
            throw new WalletNotAvailableException(walletId);
        }

        return await _walletDbContext.Transactions.Where(x => x.WalletId == walletId)
                                                  .OrderByDescending(x => x.CreatedOn)
                                                  .ToListAsync(cancellationToken);
    }
    
    public async Task<SendToWalletResponse> SendToWalletAsync(
        Guid fromWalletId,
        Guid toWalletId,
        decimal amount,
        string? description,
        CancellationToken cancellationToken)
    {
        var sendToWalletRequestValidator = new SendToWalletRequestValidator();

        await ValidateTransactionAsync(fromWalletId, amount, cancellationToken);
        
        await using var dbTransaction = await _walletDbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await _walletService.WithdrawAsync(fromWalletId, amount, cancellationToken);

            await _walletService.DepositAsync(toWalletId, amount, cancellationToken);

            var withdrawTx = Transaction.CreateWithdrawTransaction(
                fromWalletId,
                amount,
                $"Sent to wallet {toWalletId}. {description}".Trim());

            var depositTx = Transaction.CreateDepositTransaction(
                toWalletId,
                amount,
                $"Received from wallet {fromWalletId}. {description}".Trim());

            await _walletDbContext.Transactions.AddRangeAsync(new[] { withdrawTx, depositTx }, cancellationToken);
            await _walletDbContext.SaveChangesAsync(cancellationToken);

            await dbTransaction.CommitAsync(cancellationToken);

            return new SendToWalletResponse(
                fromWalletId,
                toWalletId,
                amount,
                withdrawTx.Id,
                depositTx.Id
            );
        }
        catch
        {
            await dbTransaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}