namespace AntiCheat.Client.Agent.Scanning;

using AntiCheat.Core.Models;

public interface IScanner
{
    Task<IReadOnlyList<DetectionEvent>> ScanAsync(CancellationToken cancellationToken);
}
