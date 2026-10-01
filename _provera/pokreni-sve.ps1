# Setup: LocalDB + baza VP2026Vinarija + build slojeva + testovi
# Pokretanje (desni klik -> Run with PowerShell, ili):
#   powershell -ExecutionPolicy Bypass -File "_provera\pokreni-sve.ps1"

$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
Set-Location $root
$log = Join-Path $PSScriptRoot 'setup-izlaz.txt'
function L($m) { $m | Tee-Object -FilePath $log -Append }
'' | Set-Content -Path $log -Encoding UTF8

Get-Process iisexpress -ErrorAction SilentlyContinue | Stop-Process -Force

function Convert-SqlUtf8([string]$path) {
  $bytes = [System.IO.File]::ReadAllBytes($path)
  $enc = [System.Text.Encoding]::UTF8
  if ($bytes.Length -ge 2 -and $bytes[1] -eq 0x00 -and $bytes[0] -ne 0x00) {
    $enc = [System.Text.Encoding]::Unicode
  }
  $text = [System.IO.File]::ReadAllText($path, $enc)
  $utf8 = New-Object System.Text.UTF8Encoding $false
  [System.IO.File]::WriteAllText($path, $text, $utf8)
  return $text
}

function Invoke-SqlBatches([string]$connectionString, [string]$sql) {
  $batches = [regex]::Split($sql, '(?im)^\s*GO\s*$')
  $conn = New-Object System.Data.SqlClient.SqlConnection $connectionString
  $conn.Open()
  try {
    foreach ($batch in $batches) {
      $t = $batch.Trim()
      if (-not $t) { continue }
      $cmd = $conn.CreateCommand()
      $cmd.CommandTimeout = 120
      $cmd.CommandText = $t
      [void]$cmd.ExecuteNonQuery()
    }
  } finally { $conn.Close() }
}

L '=== 1. LocalDB ==='
$localdb = Get-Command sqllocaldb -ErrorAction SilentlyContinue
if (-not $localdb) { throw 'sqllocaldb nije pronadjen. Instaliraj SQL Server Express LocalDB.' }
sqllocaldb start MSSQLLocalDB | ForEach-Object { L $_ }

L '=== 2. Baza VP2026Vinarija ==='
$kreiranje = Join-Path $root '1_SlojPodataka\BazaPodataka\KOMPLETNA BAZA PODATAKA\Skript za kreiranje baze podataka i 2 tabele.txt'
$sp = Join-Path $root '1_SlojPodataka\BazaPodataka\STORED PROCEDURE\SP.txt'
Invoke-SqlBatches 'Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Initial Catalog=master' (Convert-SqlUtf8 $kreiranje)
Invoke-SqlBatches 'Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Initial Catalog=VP2026Vinarija' (Convert-SqlUtf8 $sp)
L 'Baza OK (SORTA, TURA, PRIJEM, Korisnik, stored procedure)'

L '=== 3. Build ==='
$msbuildFx = 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe'
if (-not (Test-Path $msbuildFx)) { $msbuildFx = 'C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe' }
if (-not (Test-Path $msbuildFx)) { throw 'MSBuild nije pronadjen (.NET Framework 4).' }
$msbuildVs = @(
  "${env:ProgramFiles}\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe",
  "${env:ProgramFiles}\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
) | Where-Object { Test-Path $_ } | Select-Object -First 1
$projekti = @(
  '1_SlojPodataka\TehnoloskeKlase\DBUtils\DBUtils\DBUtils\DBUtils.csproj',
  '1_SlojPodataka\KlasePodataka\KlasePodataka\KlasePodataka\KlasePodataka.csproj',
  '3_SlojServisa\KlaseMapiranja\KlaseMapiranja\KlaseMapiranja.csproj',
  '2_SlojPoslovneLogike\PoslovnaLogika\PoslovnaLogika\PoslovnaLogika.csproj',
  '4_PrezentacioniSloj\PrezentacionaLogika\PrezentacionaLogika\PrezentacionaLogika.csproj'
)
foreach ($p in $projekti) {
  L "Build $p"
  & $msbuildFx (Join-Path $root $p) /p:Configuration=Debug /v:minimal /nologo
  if ($LASTEXITCODE -ne 0) { throw "Build nije uspeo: $p" }
}
$uiProj = '4_PrezentacioniSloj\KorisnickiInterfejs\KorisnickiInterfejs\KorisnickiInterfejs\KorisnickiInterfejs.csproj'
L "Build $uiProj"
$msbuildUi = $msbuildFx
if ($msbuildVs) { $msbuildUi = $msbuildVs }
& $msbuildUi (Join-Path $root $uiProj) /p:Configuration=Debug /v:minimal /nologo
if ($LASTEXITCODE -ne 0) { throw "Build nije uspeo: $uiProj" }
L 'Build OK'

$uiBin = Join-Path $root '4_PrezentacioniSloj\KorisnickiInterfejs\KorisnickiInterfejs\KorisnickiInterfejs\bin'
New-Item -ItemType Directory -Force -Path $uiBin | Out-Null
foreach ($d in @(
  (Join-Path $root '1_SlojPodataka\TehnoloskeKlase\DBUtils\DBUtils\DBUtils\bin\Debug\DBUtils.dll'),
  (Join-Path $root '1_SlojPodataka\KlasePodataka\KlasePodataka\KlasePodataka\bin\Debug\KlasePodataka.dll'),
  (Join-Path $root '3_SlojServisa\KlaseMapiranja\KlaseMapiranja\bin\Debug\KlaseMapiranja.dll'),
  (Join-Path $root '2_SlojPoslovneLogike\PoslovnaLogika\PoslovnaLogika\bin\Debug\PoslovnaLogika.dll'),
  (Join-Path $root '4_PrezentacioniSloj\PrezentacionaLogika\PrezentacionaLogika\bin\Debug\PrezentacionaLogika.dll'),
  (Join-Path $root 'packages\EntityFramework.6.1.3\lib\net40\EntityFramework.dll'),
  (Join-Path $root 'packages\EntityFramework.6.1.3\lib\net40\EntityFramework.SqlServer.dll')
)) {
  if (Test-Path $d) { Copy-Item $d $uiBin -Force }
}
L 'DLL kopirani u UI bin (ukljucujuci Entity Framework)'

L '=== 4. Testovi ==='
$csc = 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe'
if (-not (Test-Path $csc)) { $csc = 'C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe' }
$dlls = @(
  (Join-Path $root '1_SlojPodataka\TehnoloskeKlase\DBUtils\DBUtils\DBUtils\bin\Debug\DBUtils.dll'),
  (Join-Path $root '1_SlojPodataka\KlasePodataka\KlasePodataka\KlasePodataka\bin\Debug\KlasePodataka.dll'),
  (Join-Path $root '3_SlojServisa\KlaseMapiranja\KlaseMapiranja\bin\Debug\KlaseMapiranja.dll'),
  (Join-Path $root '2_SlojPoslovneLogike\PoslovnaLogika\PoslovnaLogika\bin\Debug\PoslovnaLogika.dll'),
  (Join-Path $root '4_PrezentacioniSloj\PrezentacionaLogika\PrezentacionaLogika\bin\Debug\PrezentacionaLogika.dll')
)
$exe = Join-Path $PSScriptRoot 'Testovi.exe'
$src = Join-Path $PSScriptRoot 'Testovi.cs'
$argList = @('/nologo', '/t:exe', "/out:$exe", '/r:System.Data.dll', '/r:System.Web.Services.dll', '/r:System.dll', '/r:System.Xml.dll', '/r:System.Configuration.dll', '/r:System.EnterpriseServices.dll') + ($dlls | ForEach-Object { "/r:$_" }) + @($src)
& $csc @argList
if ($LASTEXITCODE -ne 0) { throw 'Kompajliranje testova nije uspelo' }
foreach ($d in $dlls) { Copy-Item $d $PSScriptRoot -Force }
Copy-Item (Join-Path $root 'packages\EntityFramework.6.1.3\lib\net40\EntityFramework.dll') $PSScriptRoot -Force
Copy-Item (Join-Path $root 'packages\EntityFramework.6.1.3\lib\net40\EntityFramework.SqlServer.dll') $PSScriptRoot -Force
Copy-Item (Join-Path $root '2_SlojPoslovneLogike\PoslovnaLogika\PoslovnaLogika\app.config') (Join-Path $PSScriptRoot 'PoslovnaLogika.dll.config') -Force
$xml = Join-Path $root '3_SlojServisa\WebServis\ParametriKvaliteta\ParametriKvaliteta\XML\OgranicenjaKvaliteta.XML'
& $exe $xml
$kod = $LASTEXITCODE
L "Testovi exit=$kod"

L ''
L '=== STA SLEDI ZA MVC ==='
$vs = Get-ChildItem 'C:\Program Files\Microsoft Visual Studio','C:\Program Files (x86)\Microsoft Visual Studio' -ErrorAction SilentlyContinue
if (-not $vs) {
  L 'Visual Studio NIJE instaliran na ovom racunaru.'
  L 'Instaliraj Visual Studio 2022 Community (ASP.NET workload).'
} else {
  L "Visual Studio: $($vs.FullName -join ', ')"
}
L 'Ili pokreni: powershell -ExecutionPolicy Bypass -File "_provera\pokreni-web.ps1"'
L 'Start: http://localhost:1202/Prijava  (admin / admin)'
exit $kod
