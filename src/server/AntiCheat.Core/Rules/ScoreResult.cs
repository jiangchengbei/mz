namespace AntiCheat.Core.Rules;

public sealed record ScoreResult(
    int TotalScore,
    string Decision,
    IReadOnlyList<string> MatchedRules);
