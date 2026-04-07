namespace AntiCheat.Client.Agent.Scanning;

using AntiCheat.Core.Models;

public sealed class ScanCoordinator
{
    private readonly IReadOnlyList<IScanner> _scanners;

    public ScanCoordinator(IEnumerable<IScanner> scanners) => _scanners = scanners.ToList();

    public async Task<IReadOnlyList<DetectionEvent>> RunPreMatchScanAsync(CancellationToken cancellationToken)
    {
        var all = new List<DetectionEvent>();
        foreach (var scanner in _scanners)
        {
            all.AddRange(await scanner.ScanAsync(cancellationToken));
        }

        return all;
    }
}
