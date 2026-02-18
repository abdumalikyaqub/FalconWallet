using AutoMapper;
using FalconWallet.API.Common;
using FalconWallet.API.Features.Transactions.Common;
using Microsoft.AspNetCore.Mvc;

namespace FalconWallet.API.Features.Transactions.SendToWallet;

public static class Endpoint
{
    public static IEndpointRouteBuilder AddSendToWalletEndPoint(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/transactions/send", async (
                [FromBody] SendToWalletRequest request,
                TransactionService transactionService,
                CancellationToken cancellationToken) =>
            {
                var result = await transactionService.SendToWalletAsync(
                    request.FromWalletId,
                    request.ToWalletId,
                    request.Amount,
                    request.Description,
                    cancellationToken);

                return Results.Ok(result);
            })
            .Validator<SendToWalletRequest>()
            .WithTags(TransactionEndpointSchema.TransactionTag);

        return endpointRouteBuilder;
    }
}