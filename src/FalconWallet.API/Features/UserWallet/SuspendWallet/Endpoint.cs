using FalconWallet.API.Features.UserWallet.Common;
using Microsoft.AspNetCore.Mvc;
using FalconWallet.API.Features.UserWallet.Common.Interfaces;

namespace FalconWallet.API.Features.UserWallet.SuspendWallet;

public static class Endpoint
{
    public static IEndpointRouteBuilder AddSuspendWalletEndPoint(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPatch("/wallet/{walletId:guid:required}/suspend", async (
            [FromRoute(Name = "walletId")] Guid walletId,
            IWalletService walletService,
            CancellationToken cancellationToken) =>
        {
            await walletService.SuspendWalletAsync(walletId, cancellationToken);

            return Results.Ok("Wallet has been suspended");
        }).WithTags(WalletEndpointSchema.WalletTag);

        return endpointRouteBuilder;
    }
}
