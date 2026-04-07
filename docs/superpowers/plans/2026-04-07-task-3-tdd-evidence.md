# Task 3 TDD Evidence

## RED
Command:
`& "C:\Program Files\dotnet\dotnet.exe" test "tests/server/AntiCheat.Core.Tests/AntiCheat.Core.Tests.csproj" --filter RiskScoringServiceTests`

Result:
- FAIL
- `RuleDefinition` constructor shape did not match Task 3 spec
- `RiskScoringService` was static instead of instantiable
- `Evaluate` did not accept `(rules, detections)`
- `ScoreResult` did not expose `Decision`

## GREEN
Command:
`& "C:\Program Files\dotnet\dotnet.exe" test "tests/server/AntiCheat.Core.Tests/AntiCheat.Core.Tests.csproj" --filter RiskScoringServiceTests`

Result:
- PASS
- `Passed: 1, Failed: 0, Skipped: 0, Total: 1`

## CURRENT EXACT PLAN COMMAND CHECK
Command:
`& "C:\Program Files\dotnet\dotnet.exe" test "tests/server/AntiCheat.Core.Tests/AntiCheat.Core.Tests.csproj" --filter returns_block_when_high_risk_rule_is_confirmed`

Result:
- FAIL
- Blocked by existing compile issue in `tests/server/AntiCheat.Core.Tests/RiskContractsTests.cs`
- Error: `DetectionEvent` best overload does not contain a parameter named `category`
