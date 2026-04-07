using AntiCheat.Core.Models;

namespace AntiCheat.Core.Rules;

public sealed record RuleDefinition(
    string RuleId,
    string Category,
    int Weight,
    RiskLevel RiskLevel,
    bool AutoBlock);
