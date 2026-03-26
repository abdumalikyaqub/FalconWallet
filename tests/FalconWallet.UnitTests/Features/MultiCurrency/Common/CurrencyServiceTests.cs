using FalconWallet.API.Features.MultiCurrency.Common;
using FalconWallet.API.Features.MultiCurrency.Repositories.Interfaces;
using FluentAssertions;
using Moq;

namespace FalconWallet.UnitTests.Features.MultiCurrency.Common;

public class CurrencyServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateCurrencySuccess()
    {
        //Arrange
        var repoMock = new Mock<ICurrencyRepository>();
        repoMock.Setup(x => x.ExistsByCodeAsync("USD"))
            .ReturnsAsync(false);
        
        var service = new CurrencyService(repoMock.Object);

        //Act
        var result = await service.CreateAsync("USD", "dollar", 1.2m);
        
        //Assert
        result.Code.Should().Be("USD");
        repoMock.Verify(x => x.AddAsync(It.IsAny<Currency>()), Times.Once);
        repoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_CurrencyAlreadyExists_ShowThrowException()
    {
        //Arrange
        var repoMock = new Mock<ICurrencyRepository>();
        repoMock.Setup(x => x.ExistsByCodeAsync("USD"))
            .ReturnsAsync(true);
        
        var service = new CurrencyService(repoMock.Object);
        
        //Act
        var action = async () => await service.CreateAsync("USD", "Dollar", 1.2m);

        //Assert
        await action.Should().ThrowAsync<CurrencyAlreadyExistException>();
    }

    [Fact]
    public async Task CreateAsync_InvalidConversionRate_ShowThrowException()
    {
        //Arrange
        var repoMock = new Mock<ICurrencyRepository>();
        
        var service = new CurrencyService(repoMock.Object);
        
        //Act
        var action = async () => await service.CreateAsync("USD", "dollar", 0);

        //Assert
        await action.Should().ThrowAsync<InvalidConversionRateException>();
    }

    [Fact]
    public async Task UpdateConversionRateAsync_ShouldUpdateConversionRateSuccess()
    {
        //Arrange
        var currency = Currency.Create("USD", "USD", 1.2m);
        
        var repoMock = new Mock<ICurrencyRepository>();
        
        repoMock.Setup(x => x.GetByIdAsync(currency.Id))
            .ReturnsAsync(currency);
        
        var service = new CurrencyService(repoMock.Object);
        
        //Act
        await service.UpdateConversionRateAsync(currency.Id, 2.5m);

        //Assert
        currency.ConversionRate.Should().Be(2.5m);
        repoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateConversationRateAsync_InvalidConversationRate_ShowThrowException()
    {
        //Arrange
        var repoMock = new Mock<ICurrencyRepository>();
        
        var service = new CurrencyService(repoMock.Object);
        
        //Act
        var action = async () => await service.UpdateConversionRateAsync(1, 0);

        //Assert
        await action.Should().ThrowAsync<InvalidConversionRateException>();
    }

    [Fact]
    public async Task UpdateConversationRateAsync_CurrencyNotFound_ShowThrowException()
    {
        //Arrange
        var repoMock = new Mock<ICurrencyRepository>();

        repoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Currency?)null);
        
        var service = new CurrencyService(repoMock.Object);
        
        //Acr
        var action = async () => await service.UpdateConversionRateAsync(999, 1.2m);

        //Assert
        await action.Should().ThrowAsync<CurrencyNotFoundException>();
    }

    [Fact]
    public async Task HasByIdAsync_ShouldReturnTrue()
    {
        //Arrange
        var repoMock = new Mock<ICurrencyRepository>();
        repoMock.Setup(x => x.HasByIdAsync(1)).ReturnsAsync(true);

        var service = new CurrencyService(repoMock.Object);
        
        //Act
        var result = await service.HasByIdAsync(1);

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task HasByIdAsync_ShouldReturnFalse()
    {
        //Arrange
        var repoMock = new Mock<ICurrencyRepository>();
        repoMock.Setup(x => x.HasByIdAsync(999)).ReturnsAsync(false);
        
        var service = new CurrencyService(repoMock.Object);

        //Act
        var result = await service.HasByIdAsync(999);

        //Assert
        result.Should().BeFalse();
    }
}