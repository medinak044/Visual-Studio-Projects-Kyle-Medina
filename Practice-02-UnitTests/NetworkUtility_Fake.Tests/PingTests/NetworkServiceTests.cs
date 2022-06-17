using FluentAssertions;
using NetworkUtility_Fake.Ping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NetworkUtility_Fake.Tests.PingTests;

public class NetworkServiceTests
{
    // Remember: "Fluent" documentation exists online

    [Fact]
    public void NetworkService_SendPing_ReturnString()
    {
        // Arrange
        var pingService = new NetworkService();

        // Act
        var result = pingService.SendPing();

        // Assert (using Fluent assertions)
        result.Should().NotBeNullOrWhiteSpace();
        result.Should().Be("Success: Ping Sent!");
        result.Should().Contain("Success", Exactly.Once());
    }

    [Theory] // Let's you pass variables/inline data
    [InlineData(1, 1, 2)]
    [InlineData(2, 2, 4)]
    public void NetworkService_PingTimeOut_ReturnInt(int a, int b, int expected)
    {
        var pingService = new NetworkService();

        var result = pingService.PingTimeOut(a,b);

        result.Should().Be(expected);
        result.Should().BeGreaterThanOrEqualTo(2);
        result.Should().NotBeInRange(-10000, 0);
    }

}
