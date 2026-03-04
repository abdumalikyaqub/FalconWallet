using AutoMapper;
using FalconWallet.API.Common;
using FalconWallet.API.Features.MultiCurrency.Common;
using FalconWallet.API.Features.MultiCurrency.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FalconWallet.API.Features.MultiCurrency.CreateCurrency;

public static class Endpoint
{
    public static IEndpointRouteBuilder AddCreateCurrencyEndPoint(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/currency", async (
            [FromBody] CreateCurrencyRequest request,
            ICurrencyService currencyService,
            IMapper mapper,
            CancellationToken cancellationToken) =>
        {
            Currency currency = await currencyService.CreateAsync(request.Code, request.Name, request.ConversionRate, cancellationToken);

            return mapper.Map<CreateCurrencyResponse>(currency);
        }).Validator<CreateCurrencyRequest>()
          .WithTags(CurrencyEndpointSchema.CurrencyTag);

        return endpointRouteBuilder;
    }
}
