namespace AntiCheat.Client.Agent.Scanning;

using AntiCheat.Core.Models;

public sealed class ProcessScanner : IScanner
{
    public Task<IReadOnlyList<DetectionEvent>> ScanAsync(CancellationToken cancellationToken) =>
        Task.FromResult<IReadOnlyList<DetectionEvent>>(Array.Empty<DetectionEvent>());
}
