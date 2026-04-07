using AntiCheat.Client.Agent.Scanning;
using AntiCheat.Core.Models;
using FluentAssertions;
using Xunit;

public class ScanCoordinatorTests
{
    [Fact]
    public async Task runs_all_scanners_and_returns_flattened_detections()
    {
        var scanners = new IScanner[]
        {
            new StubScanner(new DetectionEvent("process", "PROC-001", RiskLevel.Medium, "macro tool", "review")),
            new StubScanner(new DetectionEvent("usb", "USB-009", RiskLevel.High, "unknown HID", "block"))
        };

        var result = await new ScanCoordinator(scanners).RunPreMatchScanAsync(CancellationToken.None);

        result.Should().HaveCount(2);
    }

    private sealed class StubScanner : IScanner
    {
        private readonly DetectionEvent _event;
        public StubScanner(DetectionEvent @event) => _event = @event;
        public Task<IReadOnlyList<DetectionEvent>> ScanAsync(CancellationToken cancellationToken) =>
            Task.FromResult<IReadOnlyList<DetectionEvent>>(new[] { _event });
    }
}
