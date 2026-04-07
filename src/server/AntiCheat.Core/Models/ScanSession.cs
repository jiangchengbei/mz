namespace AntiCheat.Core.Models;

public sealed record ScanSession(
    Guid SessionId,
    string PlayerId,
    string MatchId,
    DateTimeOffset StartedAtUtc,
    IReadOnlyList<DetectionEvent> Detections,
    IReadOnlyList<EvidenceArtifact> Evidence);
