using DemoSonarApp.Controllers;
using Microsoft.Extensions.Logging;
using Moq;

namespace DemoSonarApp.Tests.Controllers;

[TestClass]
public class WeatherForecastControllerTests
{
    private Mock<ILogger<WeatherForecastController>> _loggerMock = null!;
    private WeatherForecastController _controller = null!;

    private static readonly string[] ValidSummaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [TestInitialize]
    public void Setup()
    {
        // Arrange - shared setup
        _loggerMock = new Mock<ILogger<WeatherForecastController>>();
        _controller = new WeatherForecastController(_loggerMock.Object);
    }

    [TestMethod]
    public void Get_WhenCalled_ReturnsNonNullResult()
    {
        // Arrange
        // (controller initialized in Setup)

        // Act
        var result = _controller.Get();

        // Assert
        Assert.IsNotNull(result, "Result should not be null.");
    }

    [TestMethod]
    public void Get_WhenCalled_ReturnsFiveForecasts()
    {
        // Arrange
        const int expectedCount = 5;

        // Act
        var result = _controller.Get().ToList();

        // Assert
        Assert.AreEqual(expectedCount, result.Count,
            $"Expected {expectedCount} forecasts but got {result.Count}.");
    }

    [TestMethod]
    public void Get_WhenCalled_ReturnsWeatherForecastsWithFutureDates()
    {
        // Arrange
        var today = DateTime.Now.Date;

        // Act
        var result = _controller.Get().ToList();

        // Assert
        Assert.IsTrue(result.All(f => f.Date.Date > today),
            "All forecast dates should be in the future.");
    }

    [TestMethod]
    public void Get_WhenCalled_ReturnsWeatherForecastsWithTemperaturesInValidRange()
    {
        // Arrange
        const int minTemperature = -20;
        const int maxTemperature = 55;

        // Act
        var result = _controller.Get().ToList();

        // Assert
        Assert.IsTrue(result.All(f => f.TemperatureC >= minTemperature && f.TemperatureC <= maxTemperature),
            $"All temperatures should be between {minTemperature} and {maxTemperature} degrees Celsius.");
    }

    [TestMethod]
    public void Get_WhenCalled_ReturnsWeatherForecastsWithValidSummaries()
    {
        // Arrange
        // (ValidSummaries defined at class level)

        // Act
        var result = _controller.Get().ToList();

        // Assert
        Assert.IsTrue(result.All(f => ValidSummaries.Contains(f.Summary)),
            "All summaries should be from the predefined list of valid summaries.");
    }

    [TestMethod]
    public void Get_WhenCalled_ReturnsWeatherForecastsWithNonNullSummaries()
    {
        // Arrange
        // (controller initialized in Setup)

        // Act
        var result = _controller.Get().ToList();

        // Assert
        Assert.IsTrue(result.All(f => f.Summary != null),
            "All forecast summaries should be non-null.");
    }

    [TestMethod]
    public void Get_WhenCalled_ReturnsCorrectTemperatureFahrenheit()
    {
        // Arrange
        // (controller initialized in Setup)

        // Act
        var result = _controller.Get().ToList();

        // Assert
        Assert.IsTrue(result.All(f => f.TemperatureF == 32 + (int)(f.TemperatureC / 0.5556)),
            "TemperatureF should be correctly calculated from TemperatureC.");
    }

    [TestMethod]
    public void Get_WhenCalledMultipleTimes_ReturnsConsistentCount()
    {
        // Arrange
        const int expectedCount = 5;

        // Act
        var firstCall = _controller.Get().ToList();
        var secondCall = _controller.Get().ToList();

        // Assert
        Assert.AreEqual(expectedCount, firstCall.Count,
            "First call should return 5 forecasts.");
        Assert.AreEqual(expectedCount, secondCall.Count,
            "Second call should return 5 forecasts.");
    }

    [TestCleanup]
    public void Teardown()
    {
        _loggerMock.VerifyAll();
    }
}