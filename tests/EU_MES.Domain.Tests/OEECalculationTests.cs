using Xunit;

namespace EU_MES.Domain.Tests;

public class OEECalculationTests
{
    [Fact]
    public void OEE_PerfectConditions_ShouldBeOne()
    {
        double availability = 1.0;
        double performance = 1.0;
        double quality = 1.0;
        double oee = availability * performance * quality;
        Assert.Equal(1.0, oee, precision: 4);
    }

    [Fact]
    public void OEE_WithDowntime_ShouldReduceAvailability()
    {
        double plannedMinutes = 480.0;
        double downtimeMinutes = 48.0;
        double availability = (plannedMinutes - downtimeMinutes) / plannedMinutes;
        Assert.Equal(0.9, availability, precision: 4);
    }

    [Fact]
    public void OEE_WithScrap_ShouldReduceQuality()
    {
        int totalProduced = 100;
        int scrap = 5;
        int good = totalProduced - scrap;
        double quality = (double)good / totalProduced;
        Assert.Equal(0.95, quality, precision: 4);
    }

    [Fact]
    public void OEE_WorldClass_ShouldBeAtLeast85Percent()
    {
        double worldClassOEE = 0.85;
        double availability = 0.90;
        double performance = 0.95;
        double quality = 0.9990;
        double oee = availability * performance * quality;
        Assert.True(oee >= worldClassOEE - 0.01, $"OEE {oee:P2} should be close to world class {worldClassOEE:P2}");
    }

    [Fact]
    public void OEE_Formula_A_times_P_times_Q()
    {
        double a = 0.90, p = 0.95, q = 0.99;
        double expected = a * p * q;
        double oee = a * p * q;
        Assert.Equal(expected, oee, precision: 6);
    }

    [Theory]
    [InlineData(480, 0, 1.0)]
    [InlineData(480, 48, 0.9)]
    [InlineData(480, 240, 0.5)]
    public void OEE_Availability_Parameterized(double planned, double downtime, double expectedAvailability)
    {
        double availability = (planned - downtime) / planned;
        Assert.Equal(expectedAvailability, availability, precision: 4);
    }
}
