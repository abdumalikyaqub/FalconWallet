using FalconWallet.API.Common.Persistence;
using FalconWallet.API.Features.MultiCurrency.Common;
using FalconWallet.API.Features.MultiCurrency.CreateCurrency;
using FalconWallet.API.Features.MultiCurrency.UpdateConversionRate;
using FalconWallet.API.Features.Transactions.Common;
using FalconWallet.API.Features.Transactions.DepositToWallet;
using FalconWallet.API.Features.Transactions.WalletHistory;
using FalconWallet.API.Features.Transactions.WithdrawFromWallet;
using FalconWallet.API.Features.UserWallet.Common;
using FalconWallet.API.Features.UserWallet.CreateWallet;
using FalconWallet.API.Features.UserWallet.GetWallet;
using FalconWallet.API.Features.UserWallet.SuspendWallet;
using FalconWallet.API.Features.UserWallet.UpdateTitle;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies();

builder.Services.AddDbContext<WalletDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(WalletDbContextSchema.DefaultConnectionStringName)));

builder.Services.AddScoped<CurrencyService>();
builder.Services.AddScoped<WalletService>();
builder.Services.AddScoped<TransactionService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblies(domainAssemblies);

builder.Services.AddAutoMapper(_ => { }, domainAssemblies);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.AddCreateCurrencyEndPoint();
app.AddUpdateConversionRateEndPoint();
app.AddCreateWalletEndPoint();
app.AddUpdateTitleEndPoint();
app.AddSuspendWalletEndPoint();
app.AddDepositToWalletEndPoint();
app.AddWithdrawFromWalletEndPoint();
app.AddWalletHistoryEndPoint();
app.AddGetWalletEndPoint();

app.Run();