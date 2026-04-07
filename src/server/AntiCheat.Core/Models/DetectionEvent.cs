namespace AntiCheat.Core.Models;

public sealed record DetectionEvent(
    string Category,
    string RuleId,
    RiskLevel RiskLevel,
    string Summary,
    string RecommendedAction)
{
    public bool RequiresImmediateBlock() => RiskLevel == RiskLevel.High;
}
