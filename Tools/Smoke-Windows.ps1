$ErrorActionPreference = 'Stop'
Set-Location (Split-Path $PSScriptRoot -Parent)
if (-not ((Get-Content Artifacts/build-result.txt -Raw).StartsWith('Succeeded'))) { throw 'A successful Windows build is required.' }
Add-Type -AssemblyName System.Drawing
Add-Type @"
using System; using System.Runtime.InteropServices;
public class RekindlePlayerWindow {
 [StructLayout(LayoutKind.Sequential)] public struct Point { public int X,Y; }
 [StructLayout(LayoutKind.Sequential)] public struct Rect { public int L,T,R,B; }
 [DllImport("user32.dll")] public static extern bool GetClientRect(IntPtr h,out Rect r);
 [DllImport("user32.dll")] public static extern bool ClientToScreen(IntPtr h,ref Point p);
 [DllImport("user32.dll")] public static extern bool ShowWindow(IntPtr h,int n);
 [DllImport("user32.dll")] public static extern bool SetForegroundWindow(IntPtr h);
 [DllImport("user32.dll")] public static extern IntPtr GetForegroundWindow();
 [DllImport("user32.dll")] public static extern uint GetWindowThreadProcessId(IntPtr h,out uint p);
 [DllImport("kernel32.dll")] public static extern uint GetCurrentThreadId();
 [DllImport("user32.dll")] public static extern bool AttachThreadInput(uint a,uint b,bool attach);
 [DllImport("user32.dll")] public static extern bool SetCursorPos(int x,int y);
 [DllImport("user32.dll")] public static extern void mouse_event(uint f,uint x,uint y,uint d,UIntPtr e);
 [DllImport("user32.dll")] public static extern void keybd_event(byte key,byte scan,uint flags,UIntPtr extra);
 [DllImport("user32.dll")] public static extern bool SetProcessDPIAware();
}
"@
$null = [RekindlePlayerWindow]::SetProcessDPIAware()
$profile = Join-Path $env:USERPROFILE 'AppData/LocalLow/Rekindle Studio/Rekindle/rekindle-v1.json'
$hadProfile = Test-Path -LiteralPath $profile
$hadBackup = Test-Path -LiteralPath ($profile+'.bak')
if ($hadProfile) { Copy-Item -LiteralPath $profile -Destination Artifacts/CompletedJourney.json -Force }
if ($hadBackup) { Copy-Item -LiteralPath ($profile+'.bak') -Destination Artifacts/CompletedJourney.backup.json -Force }
Remove-Item -LiteralPath $profile,($profile+'.bak') -ErrorAction SilentlyContinue
function Press-Key([byte]$key, [double]$seconds) {
 [RekindlePlayerWindow]::keybd_event($key,0,0,[UIntPtr]::Zero)
 Start-Sleep -Milliseconds ([int]($seconds*1000))
 [RekindlePlayerWindow]::keybd_event($key,0,2,[UIntPtr]::Zero)
}
function Capture-Player([string]$name) {
 $r = New-Object RekindlePlayerWindow+Rect
 $origin = New-Object RekindlePlayerWindow+Point
 $null = [RekindlePlayerWindow]::GetClientRect($script:playerHandle,[ref]$r)
 $null = [RekindlePlayerWindow]::ClientToScreen($script:playerHandle,[ref]$origin)
 $bitmap = New-Object Drawing.Bitmap(($r.R-$r.L),($r.B-$r.T))
 $graphics = [Drawing.Graphics]::FromImage($bitmap)
 $graphics.CopyFromScreen($origin.X,$origin.Y,0,0,$bitmap.Size)
 $bitmap.Save((Join-Path (Get-Location) ('Artifacts/'+$name+'.png')))
 $graphics.Dispose(); $bitmap.Dispose()
}
$player = $null
try {
 $player = Start-Process -FilePath (Join-Path (Get-Location) 'Builds/Windows/Rekindle.exe') -ArgumentList @('-logFile',(Join-Path (Get-Location) 'Artifacts/windows-player.log'),'-screen-width','1280','-screen-height','720','-screen-fullscreen','0','-force-d3d11') -WindowStyle Normal -PassThru
 Write-Output "Player launched: $($player.Id)"
 $deadline = [DateTime]::UtcNow.AddSeconds(120)
 do { Start-Sleep -Milliseconds 500; $player.Refresh() } while (-not $player.HasExited -and $player.MainWindowHandle -eq 0 -and [DateTime]::UtcNow -lt $deadline)
 if ($player.HasExited -or $player.MainWindowHandle -eq 0) { throw 'Player failed to create a window.' }
 $script:playerHandle = $player.MainWindowHandle
 $null = [RekindlePlayerWindow]::ShowWindow($script:playerHandle,9)
 $null = [RekindlePlayerWindow]::SetForegroundWindow($script:playerHandle)
 Start-Sleep -Seconds 10
 $player.Refresh()
 $script:playerHandle = $player.MainWindowHandle
 $null = [RekindlePlayerWindow]::ShowWindow($script:playerHandle,9)
 $null = [RekindlePlayerWindow]::SetForegroundWindow($script:playerHandle)
 Start-Sleep -Seconds 1
 if ([RekindlePlayerWindow]::GetForegroundWindow() -ne $script:playerHandle) { throw 'Player focus blocked; no desktop screenshot or input will be captured.' }
 Capture-Player 'windows-main-menu'
 $rect = New-Object RekindlePlayerWindow+Rect
 $origin = New-Object RekindlePlayerWindow+Point
 $null = [RekindlePlayerWindow]::GetClientRect($script:playerHandle,[ref]$rect)
 $null = [RekindlePlayerWindow]::ClientToScreen($script:playerHandle,[ref]$origin)
 $null = [RekindlePlayerWindow]::SetCursorPos(($origin.X+[int]($rect.R*300/1280)),($origin.Y+[int]($rect.B*389/720)))
 [RekindlePlayerWindow]::mouse_event(2,0,0,0,[UIntPtr]::Zero)
 Start-Sleep -Milliseconds 200
 [RekindlePlayerWindow]::mouse_event(4,0,0,0,[UIntPtr]::Zero)
 Start-Sleep -Seconds 2
 Press-Key 0x57 2
 Press-Key 0x51 1
 Press-Key 0x46 0.15
 Start-Sleep -Seconds 1
 Capture-Player 'windows-first-soul'
 $null = $player.CloseMainWindow()
 if (-not $player.WaitForExit(45000)) { throw 'Player did not close within 45 seconds.' }
 if (-not (Test-Path -LiteralPath $profile)) { throw 'Standalone player did not save its journey.' }
 Copy-Item -LiteralPath $profile -Destination Artifacts/WindowsSmokeJourney.json -Force
 $snapshot = Get-Content -LiteralPath $profile -Raw | ConvertFrom-Json
 if ($snapshot.version -ne 1 -or $snapshot.z -le -8 -or $snapshot.x -ge -0.5) { throw "Movement/save check failed: x=$($snapshot.x), z=$($snapshot.z)" }
 @('PASS Windows player launches and renders a window','PASS Begin journey accepts mouse input','PASS W and Q move the standalone CharacterController','PASS Standalone writes a versioned journey snapshot','First-soul interaction and menu screenshots require visual inspection; full Windows restoration walkthrough not run.') | Set-Content Artifacts/windows-smoke.txt
 Write-Output 'Windows startup, movement, and save smoke passed.'
} catch {
 ('FAIL '+$_.Exception.Message) | Set-Content Artifacts/windows-smoke.txt
 throw
} finally {
 if ($player -and -not $player.HasExited) { $null=$player.CloseMainWindow(); if (-not $player.WaitForExit(5000)) { Stop-Process -Id $player.Id; $player.WaitForExit() } }
 foreach ($key in @(0x57,0x51,0x46)) { [RekindlePlayerWindow]::keybd_event([byte]$key,0,2,[UIntPtr]::Zero) }
 # Preserve the pre-smoke journey; the developer can archive/remove the automated completed profile separately.
 if ($hadProfile) { Copy-Item Artifacts/CompletedJourney.json -Destination $profile -Force } else { Remove-Item -LiteralPath $profile -ErrorAction SilentlyContinue }
 if ($hadBackup) { Copy-Item Artifacts/CompletedJourney.backup.json -Destination ($profile+'.bak') -Force } else { Remove-Item -LiteralPath ($profile+'.bak') -ErrorAction SilentlyContinue }
}






