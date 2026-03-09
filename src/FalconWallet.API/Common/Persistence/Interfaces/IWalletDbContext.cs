using FalconWallet.API.Features.MultiCurrency.Common;
using FalconWallet.API.Features.Transactions.Common;
using FalconWallet.API.Features.UserWallet.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FalconWallet.API.Common.Persistence.Interfaces;

public interface IWalletDbContext
{
    DbSet<Wallet> Wallets { get; }
    DbSet<Currency> Currencies { get; }
    DbSet<Transaction> Transactions { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}