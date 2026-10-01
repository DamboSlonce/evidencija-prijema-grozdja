$root = Split-Path $PSScriptRoot -Parent
Set-Location $root
$log = Join-Path $root '_provera\alat.log'
function Out-Log($m) { $m | Tee-Object -FilePath $log -Append }

'' | Set-Content $log
Out-Log '=== TOOLS ==='
Get-Command sqllocaldb, sqlcmd -ErrorAction SilentlyContinue | ForEach-Object { Out-Log ("CMD {0}={1}" -f $_.Name, $_.Source) }

$candidates = @(
  'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe',
  'C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe',
  'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe',
  'C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe',
  'C:\Program Files\IIS Express\iisexpress.exe',
  'C:\Program Files (x86)\IIS Express\iisexpress.exe'
)
foreach ($p in $candidates) { if (Test-Path $p) { Out-Log "FOUND $p" } }

Get-ChildItem 'C:\Program Files*\Microsoft Visual Studio' -ErrorAction SilentlyContinue | Select-Object -ExpandProperty FullName | ForEach-Object { Out-Log "VS $_" }
Get-ChildItem 'C:\Program Files\Microsoft SQL Server' -ErrorAction SilentlyContinue | Select-Object -ExpandProperty Name | ForEach-Object { Out-Log "SQL $_" }
Out-Log '=== DONE ==='
