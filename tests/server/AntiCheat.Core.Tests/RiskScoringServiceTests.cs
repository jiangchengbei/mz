using AntiCheat.Core.Models;
using AntiCheat.Core.Rules;
using FluentAssertions;
using Xunit;

namespace AntiCheat.Core.Tests;

public class RiskScoringServiceTests
{
    [Fact]
    public void returns_block_when_high_risk_rule_is_confirmed()
    {
        var rules = new[]
        {
            new RuleDefinition("DRV-001", "driver", 90, RiskLevel.High, true)
        };
        var detections = new[]
        {
            new DetectionEvent("driver", "DRV-001", RiskLevel.High, "unsigned driver", "review")
        };

        var result = new RiskScoringService().Evaluate(rules, detections);

        result.Decision.Should().Be("block");
        result.TotalScore.Should().Be(90);
    }
}
