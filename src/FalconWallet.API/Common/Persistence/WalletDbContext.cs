using FalconWallet.API.Common.Persistence.Interfaces;
using FalconWallet.API.Features.MultiCurrency.Common;
using FalconWallet.API.Features.Transactions.Common;
using FalconWallet.API.Features.UserWallet.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FalconWallet.API.Common.Persistence;

public class WalletDbContext(DbContextOptions<WalletDbContext> dbContextOptions)
    : DbContext(dbContextOptions), IWalletDbContext
{
    public DbSet<Wallet> Wallets => Set<Wallet>();
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        => Database.BeginTransactionAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(WalletDbContextSchema.DefaultSchema);

        var assembly = typeof(IAssemblyMarker).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
    }
}
