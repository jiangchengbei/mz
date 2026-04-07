namespace AntiCheat.Core.Models;

public sealed record EvidenceArtifact(
    string Kind,
    string Path,
    string Sha256,
    DateTimeOffset CapturedAtUtc);
