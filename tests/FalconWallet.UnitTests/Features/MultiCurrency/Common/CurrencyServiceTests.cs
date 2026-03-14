using FalconWallet.API.Features.MultiCurrency.Common;
using FalconWallet.API.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace FalconWallet.UnitTests.Features.MultiCurrency.Common;

public class CurrencyServiceTests 
{
    private readonly WalletDbContext _dbContext;
    private readonly CurrencyService _service;

    public CurrencyServiceTests()
    {
        _dbContext = CreateDbContext();
        _service = new CurrencyService(_dbContext);
    }

    private static WalletDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<WalletDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new WalletDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateCurrencySuccess()
    {
        //Arrange
        const string code = "USD";
        const string name = "USD Dollar";
        const decimal rate = 1.1m;
        
        //Act
        var action = await _service.CreateAsync(code, name, rate);
        
        //Assert
        action.Should().NotBeNull();
        action.Code.Should().Be(code);
        action.Name.Should().Be(name);
        action.ConversionRate.Should().Be(rate);

        var saved = await _dbContext.Currencies
            .FirstOrDefaultAsync(c => c.Code == code);
        
        saved.Should().NotBeNull();
        saved!.Name.Should().Be(name);
        saved.ConversionRate.Should().Be(rate);
    }

    [Fact]
    public async Task CreateAsync_CurrencyAlreadyExists_ShowThrowException()
    {
        //Arrange
        var exist = Currency.Create("USD", "USD",1.2m);
        
        await _dbContext.Currencies.AddAsync(exist);
        await _dbContext.SaveChangesAsync();
        
        //Act
        var action = async () => await  _service.CreateAsync("USD", "Dollar", 2.2m);
        
        //Assert
        await action.Should().ThrowAsync<CurrencyAlreadyExistException>();
    }

    [Fact]
    public async Task CreateAsync_InvalidConversionRate_ShowThrowException()
    {
        //Arrange
        const string code = "USD";
        const string name = "USD Dollar";
        const decimal rate = 0m;
        
        //Act
        var action = async () => await  _service.CreateAsync(code, name, rate);
        
        //Assert
        await action.Should().ThrowAsync<InvalidConversionRateException>();
    }

    [Fact]
    public async Task UpdateConversionRateAsync_ShouldUpdateConversionRateSuccess()
    {
        //Arrange
        var currency = Currency.Create("USD", "USD", 1.2m);
        
        await _dbContext.Currencies.AddAsync(currency);
        await _dbContext.SaveChangesAsync();
        
        const decimal newConversationRate = 2.5m;
        
        //Act
        await _service.UpdateConversionRateAsync(currency.Id, newConversationRate);
        
        //Assert
        var updated = await _dbContext.Currencies
            .FirstOrDefaultAsync(c => c.Id == currency.Id);
        
        updated.Should().NotBeNull();
        updated!.ConversionRate.Should().Be(newConversationRate);
    }

    [Fact]
    public async Task UpdateConversationRateAsync_InvalidConversationRate_ShowThrowException()
    {
        //Arrange
        const int currencyId = 1;
        const decimal newConversationRate = 0m;
        
        //Act
        var action = async () => await _service.UpdateConversionRateAsync(currencyId, newConversationRate);
        
        //Assert
        await action.Should().ThrowAsync<InvalidConversionRateException>();
    }

    [Fact]
    public async Task UpdateConversationRateAsync_CurrencyNotFound_ShowThrowException()
    {
        //Arrange
        const int currencyId = 999;
        
        const decimal newConversationRate = 1.5m;
        
        //Acr
        var action = async () => await _service.UpdateConversionRateAsync(currencyId, newConversationRate);
        
        //Assert
        await action.Should().ThrowAsync<CurrencyNotFoundException>();
    }

    [Fact]
    public async Task HasByIdAsync_ShouldReturnTrue()
    {
        //Arrange
        var currency = Currency.Create("USD", "USD", 1.2m);
        
        await  _dbContext.Currencies.AddAsync(currency);
        await _dbContext.SaveChangesAsync();
        
        //Act
        var action = await _service.HasByIdAsync(currency.Id);
        
        //Assert
        action.Should().BeTrue();
    }

    [Fact]
    public async Task HasByIdAsync_ShouldReturnFalse()
    {
        //Arrange
        const int currencyId = 999;
        
        //Act
        var action = await _service.HasByIdAsync(currencyId);
        
        //Assert
        action.Should().BeFalse();
    }
}