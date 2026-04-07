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
