using EarthQuake.Computations.Seismology.SeismicRateChange;
using EarthQuake.Persistence.Repository.Abstractions;
using Moq;
using Xunit;

namespace EarthQuake.UnitTests.Computations;

public class SeismicRateChangeServiceTest
{
    [Fact]
    public void GetRateChange_ReturnsCorrectPercentageChange()
    {
        //Arrange
        Mock<IDataQueryRepository> dataQueryRepo = new();
        SeismicRateChangeService? service = new SeismicRateChangeService(dataQueryRepo.Object);
        double r1 = 100;   
        double r2 = 150;   

        double expected = 50;

        //Act
        double result = service.GetRateChange(r1, r2);

        //Assert
        Assert.Equal(expected, result);
    }
}
