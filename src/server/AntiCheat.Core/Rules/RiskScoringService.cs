using AntiCheat.Core.Models;

namespace AntiCheat.Core.Rules;

public sealed class RiskScoringService
{
    public ScoreResult Evaluate(IEnumerable<RuleDefinition> rules, IEnumerable<DetectionEvent> detections)
    {
        var matchedRules =
            from detection in detections
            join rule in rules on detection.RuleId equals rule.RuleId
            select rule;

        var matchedRuleList = matchedRules.ToArray();
        var totalScore = matchedRuleList.Sum(rule => rule.Weight);
        var decision = matchedRuleList.Any(rule => rule.AutoBlock || rule.RiskLevel == RiskLevel.High)
            ? "block"
            : totalScore >= 60
                ? "review"
                : "allow";

        return new ScoreResult(
            totalScore,
            decision,
            matchedRuleList.Select(rule => rule.RuleId).ToArray());
    }
}
