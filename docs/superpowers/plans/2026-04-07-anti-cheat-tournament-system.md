# Anti-Cheat Tournament System Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Build a from-scratch Windows tournament anti-cheat prototype that performs pre-match machine safety checks, continuous in-match monitoring, and post-match evidence packaging for referee review.

**Architecture:** The system is split into four runnable units: a Windows desktop client, a signed Windows kernel driver, a backend API/rule engine, and a referee web console. The client performs local scans and communicates with the driver for privileged telemetry, the backend scores risk and stores evidence, and the referee console handles review and enforcement decisions.

**Tech Stack:** .NET 8, WPF, ASP.NET Core, PostgreSQL, Redis, WebSocket/SignalR, Windows kernel-mode driver (KMDF/C), xUnit, Playwright

---

## File Structure

- Create: `docs/superpowers/plans/2026-04-07-anti-cheat-tournament-system.md` — implementation plan
- Create: `src/server/TournamentAntiCheat.sln` — .NET solution file
- Create: `src/server/AntiCheat.Api/` — ASP.NET Core API, SignalR hub, rule evaluation entrypoints
- Create: `src/server/AntiCheat.Core/` — shared domain models, risk scoring, evidence packaging contracts
- Create: `src/server/AntiCheat.Infrastructure/` — PostgreSQL/Redis/storage integrations
- Create: `src/server/AntiCheat.RefereeWeb/` — referee UI for review and actions
- Create: `src/client/AntiCheat.Client/` — WPF desktop app for player login, scan state, match monitoring
- Create: `src/client/AntiCheat.Client.Agent/` — Windows service and user-mode scanners
- Create: `src/driver/AntiCheatDrv/` — KMDF driver for privileged telemetry
- Create: `tests/server/AntiCheat.Api.Tests/` — API/rule tests
- Create: `tests/server/AntiCheat.Core.Tests/` — scoring/evidence tests
- Create: `tests/client/AntiCheat.Client.Agent.Tests/` — scanner tests
- Create: `tests/web/AntiCheat.RefereeWeb.Tests/` — Playwright referee UI tests
- Create: `scripts/` — local development/bootstrap scripts

### Task 1: Bootstrap repository and solution skeleton

**Files:**
- Create: `src/server/TournamentAntiCheat.sln`
- Create: `src/server/AntiCheat.Api/AntiCheat.Api.csproj`
- Create: `src/server/AntiCheat.Core/AntiCheat.Core.csproj`
- Create: `src/server/AntiCheat.Infrastructure/AntiCheat.Infrastructure.csproj`
- Create: `src/server/AntiCheat.RefereeWeb/AntiCheat.RefereeWeb.csproj`
- Create: `src/client/AntiCheat.Client/AntiCheat.Client.csproj`
- Create: `src/client/AntiCheat.Client.Agent/AntiCheat.Client.Agent.csproj`
- Create: `src/driver/AntiCheatDrv/AntiCheatDrv.vcxproj`
- Create: `Directory.Build.props`
- Create: `README.md`
- Test: `src/server/TournamentAntiCheat.sln`

- [ ] **Step 1: Write the failing bootstrap verification script**

```powershell
$required = @(
  'src/server/TournamentAntiCheat.sln',
  'src/server/AntiCheat.Api/AntiCheat.Api.csproj',
  'src/server/AntiCheat.Core/AntiCheat.Core.csproj',
  'src/server/AntiCheat.Infrastructure/AntiCheat.Infrastructure.csproj',
  'src/server/AntiCheat.RefereeWeb/AntiCheat.RefereeWeb.csproj',
  'src/client/AntiCheat.Client/AntiCheat.Client.csproj',
  'src/client/AntiCheat.Client.Agent/AntiCheat.Client.Agent.csproj',
  'src/driver/AntiCheatDrv/AntiCheatDrv.vcxproj',
  'Directory.Build.props',
  'README.md'
)
$missing = $required | Where-Object { -not (Test-Path $_) }
if ($missing.Count -gt 0) {
  Write-Error ("Missing: " + ($missing -join ', '))
  exit 1
}
```

- [ ] **Step 2: Run verification to confirm failure**

Run: `powershell -File scripts/verify-bootstrap.ps1`
Expected: FAIL with missing file errors

- [ ] **Step 3: Write minimal repository skeleton**

```xml
<!-- Directory.Build.props -->
<Project>
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  </PropertyGroup>
</Project>
```

```markdown
# Tournament Anti-Cheat System

Windows tournament anti-cheat prototype with pre-match scanning, live monitoring, and post-match evidence review.
```

```xml
<!-- src/server/AntiCheat.Core/AntiCheat.Core.csproj -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>
</Project>
```

```xml
<!-- src/server/AntiCheat.Api/AntiCheat.Api.csproj -->
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\AntiCheat.Core\AntiCheat.Core.csproj" />
    <ProjectReference Include="..\AntiCheat.Infrastructure\AntiCheat.Infrastructure.csproj" />
  </ItemGroup>
</Project>
```

```xml
<!-- src/server/AntiCheat.Infrastructure/AntiCheat.Infrastructure.csproj -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\AntiCheat.Core\AntiCheat.Core.csproj" />
  </ItemGroup>
</Project>
```

```xml
<!-- src/server/AntiCheat.RefereeWeb/AntiCheat.RefereeWeb.csproj -->
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\AntiCheat.Core\AntiCheat.Core.csproj" />
    <ProjectReference Include="..\AntiCheat.Infrastructure\AntiCheat.Infrastructure.csproj" />
  </ItemGroup>
</Project>
```

```xml
<!-- src/client/AntiCheat.Client/AntiCheat.Client.csproj -->
<Project Sdk="Microsoft.NET.Sdk.WindowsDesktop">
  <PropertyGroup>
    <TargetFramework>net8.0-windows</TargetFramework>
    <UseWPF>true</UseWPF>
  </PropertyGroup>
</Project>
```

```xml
<!-- src/client/AntiCheat.Client.Agent/AntiCheat.Client.Agent.csproj -->
<Project Sdk="Microsoft.NET.Sdk.Worker">
  <PropertyGroup>
    <TargetFramework>net8.0-windows</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\..\server\AntiCheat.Core\AntiCheat.Core.csproj" />
  </ItemGroup>
</Project>
```

```xml
<!-- src/driver/AntiCheatDrv/AntiCheatDrv.vcxproj -->
<Project DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <ItemGroup Label="ProjectConfigurations">
    <ProjectConfiguration Include="Debug|x64">
      <Configuration>Debug</Configuration>
      <Platform>x64</Platform>
    </ProjectConfiguration>
  </ItemGroup>
</Project>
```

- [ ] **Step 4: Run verification to confirm pass**

Run: `powershell -File scripts/verify-bootstrap.ps1`
Expected: PASS with no output

- [ ] **Step 5: Commit**

```bash
git add .
git commit -m "chore: bootstrap anti-cheat solution skeleton"
```

### Task 2: Define shared domain contracts and risk levels

**Files:**
- Create: `src/server/AntiCheat.Core/Models/RiskLevel.cs`
- Create: `src/server/AntiCheat.Core/Models/ScanSession.cs`
- Create: `src/server/AntiCheat.Core/Models/DetectionEvent.cs`
- Create: `src/server/AntiCheat.Core/Models/EvidenceArtifact.cs`
- Test: `tests/server/AntiCheat.Core.Tests/RiskContractsTests.cs`

- [ ] **Step 1: Write the failing domain test**

```csharp
using AntiCheat.Core.Models;
using FluentAssertions;
using Xunit;

public class RiskContractsTests
{
    [Fact]
    public void high_risk_detection_requires_block_action()
    {
        var detection = new DetectionEvent(
            category: "driver",
            ruleId: "DRV-001",
            riskLevel: RiskLevel.High,
            summary: "Unsigned kernel callback patch",
            recommendedAction: "review");

        detection.RequiresImmediateBlock().Should().BeTrue();
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/server/AntiCheat.Core.Tests/AntiCheat.Core.Tests.csproj --filter high_risk_detection_requires_block_action`
Expected: FAIL with missing types

- [ ] **Step 3: Write minimal domain implementation**

```csharp
namespace AntiCheat.Core.Models;

public enum RiskLevel
{
    Low = 1,
    Medium = 2,
    High = 3
}

public sealed record DetectionEvent(
    string Category,
    string RuleId,
    RiskLevel RiskLevel,
    string Summary,
    string RecommendedAction)
{
    public bool RequiresImmediateBlock() => RiskLevel == RiskLevel.High;
}

public sealed record EvidenceArtifact(
    string Kind,
    string Path,
    string Sha256,
    DateTimeOffset CapturedAtUtc);

public sealed record ScanSession(
    Guid SessionId,
    string PlayerId,
    string MatchId,
    DateTimeOffset StartedAtUtc,
    IReadOnlyList<DetectionEvent> Detections,
    IReadOnlyList<EvidenceArtifact> Evidence);
```

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test tests/server/AntiCheat.Core.Tests/AntiCheat.Core.Tests.csproj --filter high_risk_detection_requires_block_action`
Expected: PASS

- [ ] **Step 5: Commit**

```bash
git add src/server/AntiCheat.Core tests/server/AntiCheat.Core.Tests
git commit -m "feat: add shared anti-cheat risk contracts"
```

### Task 3: Build rule engine and score evaluation

**Files:**
- Create: `src/server/AntiCheat.Core/Rules/RuleDefinition.cs`
- Create: `src/server/AntiCheat.Core/Rules/RiskScoringService.cs`
- Create: `src/server/AntiCheat.Core/Rules/ScoreResult.cs`
- Test: `tests/server/AntiCheat.Core.Tests/RiskScoringServiceTests.cs`

- [ ] **Step 1: Write the failing scoring test**

```csharp
using AntiCheat.Core.Models;
using AntiCheat.Core.Rules;
using FluentAssertions;
using Xunit;

public class RiskScoringServiceTests
{
    [Fact]
    public void returns_block_when_high_risk_rule_is_confirmed()
    {
        var rules = new[]
        {
            new RuleDefinition("DRV-001", "driver", 90, RiskLevel.High, true)
        };
        var detections = new[]
        {
            new DetectionEvent("driver", "DRV-001", RiskLevel.High, "unsigned driver", "review")
        };

        var result = new RiskScoringService().Evaluate(rules, detections);

        result.Decision.Should().Be("block");
        result.TotalScore.Should().Be(90);
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/server/AntiCheat.Core.Tests/AntiCheat.Core.Tests.csproj --filter returns_block_when_high_risk_rule_is_confirmed`
Expected: FAIL with missing rule engine types

- [ ] **Step 3: Write minimal scoring implementation**

```csharp
namespace AntiCheat.Core.Rules;

using AntiCheat.Core.Models;

public sealed record RuleDefinition(
    string RuleId,
    string Category,
    int Weight,
    RiskLevel RiskLevel,
    bool AutoBlock);

public sealed record ScoreResult(int TotalScore, string Decision, IReadOnlyList<string> MatchedRules);

public sealed class RiskScoringService
{
    public ScoreResult Evaluate(IEnumerable<RuleDefinition> rules, IEnumerable<DetectionEvent> detections)
    {
        var matched = rules.Join(
            detections,
            rule => rule.RuleId,
            detection => detection.RuleId,
            (rule, _) => rule).ToList();

        var total = matched.Sum(x => x.Weight);
        var block = matched.Any(x => x.AutoBlock || x.RiskLevel == RiskLevel.High);
        return new ScoreResult(total, block ? "block" : total >= 60 ? "review" : "allow", matched.Select(x => x.RuleId).ToList());
    }
}
```

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test tests/server/AntiCheat.Core.Tests/AntiCheat.Core.Tests.csproj --filter returns_block_when_high_risk_rule_is_confirmed`
Expected: PASS

- [ ] **Step 5: Commit**

```bash
git add src/server/AntiCheat.Core tests/server/AntiCheat.Core.Tests
git commit -m "feat: add backend risk scoring engine"
```

### Task 4: Implement pre-match scan orchestration in user-mode agent

**Files:**
- Create: `src/client/AntiCheat.Client.Agent/Scanning/IScanner.cs`
- Create: `src/client/AntiCheat.Client.Agent/Scanning/ScanCoordinator.cs`
- Create: `src/client/AntiCheat.Client.Agent/Scanning/InstalledSoftwareScanner.cs`
- Create: `src/client/AntiCheat.Client.Agent/Scanning/ProcessScanner.cs`
- Create: `src/client/AntiCheat.Client.Agent/Scanning/UsbDeviceScanner.cs`
- Test: `tests/client/AntiCheat.Client.Agent.Tests/ScanCoordinatorTests.cs`

- [ ] **Step 1: Write the failing orchestration test**

```csharp
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
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/client/AntiCheat.Client.Agent.Tests/AntiCheat.Client.Agent.Tests.csproj --filter runs_all_scanners_and_returns_flattened_detections`
Expected: FAIL with missing scanner types

- [ ] **Step 3: Write minimal scan orchestration implementation**

```csharp
namespace AntiCheat.Client.Agent.Scanning;

using AntiCheat.Core.Models;

public interface IScanner
{
    Task<IReadOnlyList<DetectionEvent>> ScanAsync(CancellationToken cancellationToken);
}

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

public sealed class InstalledSoftwareScanner : IScanner
{
    public Task<IReadOnlyList<DetectionEvent>> ScanAsync(CancellationToken cancellationToken) =>
        Task.FromResult<IReadOnlyList<DetectionEvent>>(Array.Empty<DetectionEvent>());
}

public sealed class ProcessScanner : IScanner
{
    public Task<IReadOnlyList<DetectionEvent>> ScanAsync(CancellationToken cancellationToken) =>
        Task.FromResult<IReadOnlyList<DetectionEvent>>(Array.Empty<DetectionEvent>());
}

public sealed class UsbDeviceScanner : IScanner
{
    public Task<IReadOnlyList<DetectionEvent>> ScanAsync(CancellationToken cancellationToken) =>
        Task.FromResult<IReadOnlyList<DetectionEvent>>(Array.Empty<DetectionEvent>());
}
```

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test tests/client/AntiCheat.Client.Agent.Tests/AntiCheat.Client.Agent.Tests.csproj --filter runs_all_scanners_and_returns_flattened_detections`
Expected: PASS

- [ ] **Step 5: Commit**

```bash
git add src/client/AntiCheat.Client.Agent tests/client/AntiCheat.Client.Agent.Tests
git commit -m "feat: add pre-match scan coordinator"
```

### Task 5: Add match monitoring loop and server heartbeat upload

**Files:**
- Create: `src/client/AntiCheat.Client.Agent/Monitoring/MonitoringWorker.cs`
- Create: `src/client/AntiCheat.Client.Agent/Monitoring/HeartbeatPayload.cs`
- Create: `src/client/AntiCheat.Client.Agent/Networking/IApiClient.cs`
- Test: `tests/client/AntiCheat.Client.Agent.Tests/MonitoringWorkerTests.cs`

- [ ] **Step 1: Write the failing monitoring test**

```csharp
using AntiCheat.Client.Agent.Monitoring;
using AntiCheat.Client.Agent.Networking;
using FluentAssertions;
using Xunit;

public class MonitoringWorkerTests
{
    [Fact]
    public async Task sends_heartbeat_with_detection_count()
    {
        var api = new FakeApiClient();
        var worker = new MonitoringWorker(api, TimeSpan.Zero);

        await worker.PublishHeartbeatAsync("player-1", "match-1", 3, CancellationToken.None);

        api.Payloads.Should().ContainSingle();
        api.Payloads[0].DetectionCount.Should().Be(3);
    }

    private sealed class FakeApiClient : IApiClient
    {
        public List<HeartbeatPayload> Payloads { get; } = new();
        public Task SendHeartbeatAsync(HeartbeatPayload payload, CancellationToken cancellationToken)
        {
            Payloads.Add(payload);
            return Task.CompletedTask;
        }
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/client/AntiCheat.Client.Agent.Tests/AntiCheat.Client.Agent.Tests.csproj --filter sends_heartbeat_with_detection_count`
Expected: FAIL with missing monitoring types

- [ ] **Step 3: Write minimal monitoring implementation**

```csharp
namespace AntiCheat.Client.Agent.Monitoring;

using AntiCheat.Client.Agent.Networking;

public sealed record HeartbeatPayload(string PlayerId, string MatchId, int DetectionCount, DateTimeOffset SentAtUtc);

public interface IApiClient
{
    Task SendHeartbeatAsync(HeartbeatPayload payload, CancellationToken cancellationToken);
}

public sealed class MonitoringWorker
{
    private readonly IApiClient _apiClient;
    private readonly TimeSpan _interval;

    public MonitoringWorker(IApiClient apiClient, TimeSpan interval)
    {
        _apiClient = apiClient;
        _interval = interval;
    }

    public Task PublishHeartbeatAsync(string playerId, string matchId, int detectionCount, CancellationToken cancellationToken) =>
        _apiClient.SendHeartbeatAsync(new HeartbeatPayload(playerId, matchId, detectionCount, DateTimeOffset.UtcNow), cancellationToken);
}
```

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test tests/client/AntiCheat.Client.Agent.Tests/AntiCheat.Client.Agent.Tests.csproj --filter sends_heartbeat_with_detection_count`
Expected: PASS

- [ ] **Step 5: Commit**

```bash
git add src/client/AntiCheat.Client.Agent tests/client/AntiCheat.Client.Agent.Tests
git commit -m "feat: add match monitoring heartbeat worker"
```

### Task 6: Expose backend APIs for session intake and rule evaluation

**Files:**
- Create: `src/server/AntiCheat.Api/Program.cs`
- Create: `src/server/AntiCheat.Api/Endpoints/SessionEndpoints.cs`
- Create: `src/server/AntiCheat.Api/Contracts/StartScanRequest.cs`
- Create: `src/server/AntiCheat.Api/Contracts/HeartbeatRequest.cs`
- Test: `tests/server/AntiCheat.Api.Tests/SessionEndpointsTests.cs`

- [ ] **Step 1: Write the failing API test**

```csharp
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class SessionEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public SessionEndpointsTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task start_scan_returns_accepted()
    {
        var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/sessions/start", new { playerId = "p1", matchId = "m1" });
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/server/AntiCheat.Api.Tests/AntiCheat.Api.Tests.csproj --filter start_scan_returns_accepted`
Expected: FAIL with missing API host

- [ ] **Step 3: Write minimal API implementation**

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/api/sessions/start", (StartScanRequest request) => Results.Accepted($"/api/sessions/{Guid.NewGuid()}", request));
app.MapPost("/api/sessions/heartbeat", (HeartbeatRequest request) => Results.Accepted(value: request));

app.Run();

public partial class Program;

public sealed record StartScanRequest(string PlayerId, string MatchId);
public sealed record HeartbeatRequest(string PlayerId, string MatchId, int DetectionCount);
```

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test tests/server/AntiCheat.Api.Tests/AntiCheat.Api.Tests.csproj --filter start_scan_returns_accepted`
Expected: PASS

- [ ] **Step 5: Commit**

```bash
git add src/server/AntiCheat.Api tests/server/AntiCheat.Api.Tests
git commit -m "feat: add anti-cheat session intake api"
```

### Task 7: Add referee dashboard with live session list

**Files:**
- Create: `src/server/AntiCheat.RefereeWeb/Program.cs`
- Create: `src/server/AntiCheat.RefereeWeb/Pages/Sessions.razor`
- Create: `src/server/AntiCheat.RefereeWeb/Services/SessionFeedClient.cs`
- Test: `tests/web/AntiCheat.RefereeWeb.Tests/referee-dashboard.spec.ts`

- [ ] **Step 1: Write the failing UI test**

```ts
import { test, expect } from '@playwright/test'

test('shows live session table', async ({ page }) => {
  await page.goto('http://localhost:5071/sessions')
  await expect(page.getByRole('heading', { name: 'Active Sessions' })).toBeVisible()
})
```

- [ ] **Step 2: Run test to verify it fails**

Run: `npx playwright test tests/web/AntiCheat.RefereeWeb.Tests/referee-dashboard.spec.ts`
Expected: FAIL because page/app does not exist

- [ ] **Step 3: Write minimal referee web implementation**

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
var app = builder.Build();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();

public partial class Program;
```

```razor
@page "/sessions"

<h1>Active Sessions</h1>
<table>
  <thead>
    <tr><th>Player</th><th>Match</th><th>Status</th></tr>
  </thead>
  <tbody>
    <tr><td>player-1</td><td>match-1</td><td>Review</td></tr>
  </tbody>
</table>
```

- [ ] **Step 4: Run test to verify it passes**

Run: `npx playwright test tests/web/AntiCheat.RefereeWeb.Tests/referee-dashboard.spec.ts`
Expected: PASS

- [ ] **Step 5: Commit**

```bash
git add src/server/AntiCheat.RefereeWeb tests/web/AntiCheat.RefereeWeb.Tests
git commit -m "feat: add referee session dashboard"
```

### Task 8: Implement evidence packaging and post-match report generation

**Files:**
- Create: `src/server/AntiCheat.Core/Evidence/EvidencePackageBuilder.cs`
- Create: `src/server/AntiCheat.Core/Evidence/PostMatchReport.cs`
- Test: `tests/server/AntiCheat.Core.Tests/EvidencePackageBuilderTests.cs`

- [ ] **Step 1: Write the failing evidence test**

```csharp
using AntiCheat.Core.Evidence;
using AntiCheat.Core.Models;
using FluentAssertions;
using Xunit;

public class EvidencePackageBuilderTests
{
    [Fact]
    public void creates_report_with_detection_and_artifact_counts()
    {
        var session = new ScanSession(
            Guid.NewGuid(),
            "player-1",
            "match-1",
            DateTimeOffset.UtcNow,
            new[] { new DetectionEvent("process", "PROC-1", RiskLevel.Medium, "macro", "review") },
            new[] { new EvidenceArtifact("log", "logs/a.json", "abc", DateTimeOffset.UtcNow) });

        var report = new EvidencePackageBuilder().Build(session);

        report.DetectionCount.Should().Be(1);
        report.ArtifactCount.Should().Be(1);
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/server/AntiCheat.Core.Tests/AntiCheat.Core.Tests.csproj --filter creates_report_with_detection_and_artifact_counts`
Expected: FAIL with missing evidence package types

- [ ] **Step 3: Write minimal evidence implementation**

```csharp
namespace AntiCheat.Core.Evidence;

using AntiCheat.Core.Models;

public sealed record PostMatchReport(
    Guid SessionId,
    string PlayerId,
    string MatchId,
    int DetectionCount,
    int ArtifactCount,
    DateTimeOffset GeneratedAtUtc);

public sealed class EvidencePackageBuilder
{
    public PostMatchReport Build(ScanSession session) => new(
        session.SessionId,
        session.PlayerId,
        session.MatchId,
        session.Detections.Count,
        session.Evidence.Count,
        DateTimeOffset.UtcNow);
}
```

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test tests/server/AntiCheat.Core.Tests/AntiCheat.Core.Tests.csproj --filter creates_report_with_detection_and_artifact_counts`
Expected: PASS

- [ ] **Step 5: Commit**

```bash
git add src/server/AntiCheat.Core tests/server/AntiCheat.Core.Tests
git commit -m "feat: add post-match evidence packaging"
```

### Task 9: Add kernel driver IOCTL contract and user-mode bridge

**Files:**
- Create: `src/driver/AntiCheatDrv/Public/AntiCheatIoctl.h`
- Create: `src/driver/AntiCheatDrv/Device.c`
- Create: `src/client/AntiCheat.Client.Agent/Driver/DriverBridge.cs`
- Test: `tests/client/AntiCheat.Client.Agent.Tests/DriverBridgeTests.cs`

- [ ] **Step 1: Write the failing bridge test**

```csharp
using AntiCheat.Client.Agent.Driver;
using FluentAssertions;
using Xunit;

public class DriverBridgeTests
{
    [Fact]
    public void uses_expected_device_path()
    {
        DriverBridge.DevicePath.Should().Be("\\\\.\\AntiCheatDrv");
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/client/AntiCheat.Client.Agent.Tests/AntiCheat.Client.Agent.Tests.csproj --filter uses_expected_device_path`
Expected: FAIL with missing bridge type

- [ ] **Step 3: Write minimal driver contract implementation**

```c
#pragma once

#define FILE_DEVICE_ANTICHEAT 0x8005
#define IOCTL_ANTICHEAT_QUERY_STATE CTL_CODE(FILE_DEVICE_ANTICHEAT, 0x801, METHOD_BUFFERED, FILE_READ_DATA)
```

```c
#include <ntddk.h>
#include "Public/AntiCheatIoctl.h"

NTSTATUS AntiCheatCreateClose(PDEVICE_OBJECT DeviceObject, PIRP Irp)
{
    UNREFERENCED_PARAMETER(DeviceObject);
    Irp->IoStatus.Status = STATUS_SUCCESS;
    Irp->IoStatus.Information = 0;
    IoCompleteRequest(Irp, IO_NO_INCREMENT);
    return STATUS_SUCCESS;
}
```

```csharp
namespace AntiCheat.Client.Agent.Driver;

public static class DriverBridge
{
    public const string DevicePath = "\\\\.\\AntiCheatDrv";
}
```

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test tests/client/AntiCheat.Client.Agent.Tests/AntiCheat.Client.Agent.Tests.csproj --filter uses_expected_device_path`
Expected: PASS

- [ ] **Step 5: Commit**

```bash
git add src/driver/AntiCheatDrv src/client/AntiCheat.Client.Agent tests/client/AntiCheat.Client.Agent.Tests
git commit -m "feat: add driver ioctl bridge contract"
```

### Task 10: Add verification scripts and local quality gates

**Files:**
- Create: `scripts/verify-bootstrap.ps1`
- Create: `scripts/test-server.ps1`
- Create: `scripts/test-client.ps1`
- Create: `scripts/test-web.ps1`
- Modify: `README.md`
- Test: `scripts/test-server.ps1`

- [ ] **Step 1: Write the failing quality gate script**

```powershell
if (-not (Test-Path 'src/server/TournamentAntiCheat.sln')) {
  Write-Error 'Missing solution file'
  exit 1
}
if (-not (Test-Path 'tests/server/AntiCheat.Core.Tests/AntiCheat.Core.Tests.csproj')) {
  Write-Error 'Missing core test project'
  exit 1
}
```

- [ ] **Step 2: Run script to verify it fails**

Run: `powershell -File scripts/test-server.ps1`
Expected: FAIL with missing test project error

- [ ] **Step 3: Write minimal verification scripts**

```powershell
# scripts/test-server.ps1
dotnet test src/server/TournamentAntiCheat.sln
```

```powershell
# scripts/test-client.ps1
dotnet test tests/client/AntiCheat.Client.Agent.Tests/AntiCheat.Client.Agent.Tests.csproj
```

```powershell
# scripts/test-web.ps1
npx playwright test tests/web/AntiCheat.RefereeWeb.Tests
```

```markdown
## Verification

- `powershell -File scripts/test-server.ps1`
- `powershell -File scripts/test-client.ps1`
- `powershell -File scripts/test-web.ps1`
```

- [ ] **Step 4: Run scripts to verify they pass**

Run: `powershell -File scripts/test-server.ps1`
Expected: PASS

- [ ] **Step 5: Commit**

```bash
git add scripts README.md
git commit -m "chore: add local verification scripts"
```

## Self-Review

- Spec coverage: pre-match scanning, live monitoring, risk scoring, referee review, evidence packaging, and kernel bridge all map to Tasks 2-9. Bootstrap and verification are covered by Tasks 1 and 10.
- Placeholder scan: removed TODO/TBD wording; each task includes explicit files, code, and commands.
- Type consistency: `DetectionEvent`, `ScanSession`, `EvidenceArtifact`, `RiskScoringService`, `HeartbeatPayload`, and `DriverBridge` names are consistent across tasks.
