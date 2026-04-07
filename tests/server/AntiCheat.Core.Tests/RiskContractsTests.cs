using AntiCheat.Core.Models;
using FluentAssertions;
using Xunit;

namespace AntiCheat.Core.Tests;

public class RiskContractsTests
{
    [Fact]
    public void high_risk_detection_requires_block_action()
    {
        var detection = new DetectionEvent(
            Category: "driver",
            RuleId: "DRV-001",
            RiskLevel: RiskLevel.High,
            Summary: "Unsigned kernel callback patch",
            RecommendedAction: "review");

        detection.RequiresImmediateBlock().Should().BeTrue();
    }
}
