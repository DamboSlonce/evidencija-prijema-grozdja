$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
Set-Location $root
$log = Join-Path $PSScriptRoot 'izlaz.txt'
function L($m) { $m | Tee-Object -FilePath $log -Append }

'' | Set-Content -Path $log -Encoding UTF8
L '=== 1. ALATI ==='

function Find-Exe($name, $paths) {
  $c = Get-Command $name -ErrorAction SilentlyContinue
  if ($c) { return $c.Source }
  foreach ($p in $paths) { if (Test-Path $p) { return $p } }
  return $null
}

$sqlLocalDb = Find-Exe 'sqllocaldb' @(
  'C:\Program Files\Microsoft SQL Server\150\Tools\Binn\SqlLocalDB.exe',
  'C:\Program Files\Microsoft SQL Server\160\Tools\Binn\SqlLocalDB.exe'
)
$sqlcmd = Find-Exe 'sqlcmd' @(
  'C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\170\Tools\Binn\SQLCMD.EXE',
  'C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\180\Tools\Binn\SQLCMD.EXE',
  'C:\Program Files\Microsoft SQL Server\110\Tools\Binn\SQLCMD.EXE'
)
$msbuild = Find-Exe 'msbuild' @(
  'C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe',
  'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe',
  'C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe',
  'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe',
  'C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe'
)
$csc = Find-Exe 'csc' @(
  'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe',
  'C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe'
)

L "sqllocaldb=$sqlLocalDb"
L "sqlcmd=$sqlcmd"
L "msbuild=$msbuild"
L "csc=$csc"

if (-not $sqlcmd) { throw 'sqlcmd nije pronadjen' }
if (-not $msbuild) { throw 'MSBuild nije pronadjen' }
if (-not $csc) { throw 'csc nije pronadjen' }

L '=== 2. BAZA ==='
if ($sqlLocalDb) {
  & $sqlLocalDb start MSSQLLocalDB | Out-String | ForEach-Object { L $_ }
}

$kreiranje = Join-Path $root '1_SlojPodataka\BazaPodataka\KOMPLETNA BAZA PODATAKA\Skript za kreiranje baze podataka i 2 tabele.txt'
$sp = Join-Path $root '1_SlojPodataka\BazaPodataka\STORED PROCEDURE\SP.txt'
& $sqlcmd -S '(localdb)\MSSQLLocalDB' -E -b -i $kreiranje
if ($LASTEXITCODE -ne 0) { throw "Kreiranje baze nije uspelo: $LASTEXITCODE" }
& $sqlcmd -S '(localdb)\MSSQLLocalDB' -d VP2026Vinarija -E -b -i $sp
if ($LASTEXITCODE -ne 0) { throw "Stored procedure nisu uspele: $LASTEXITCODE" }
L 'Baza OK'

L '=== 3. BUILD ==='
$projekti = @(
  '1_SlojPodataka\TehnoloskeKlase\DBUtils\DBUtils\DBUtils\DBUtils.csproj',
  '1_SlojPodataka\KlasePodataka\KlasePodataka\KlasePodataka\KlasePodataka.csproj',
  '3_SlojServisa\KlaseMapiranja\KlaseMapiranja\KlaseMapiranja.csproj',
  '2_SlojPoslovneLogike\PoslovnaLogika\PoslovnaLogika\PoslovnaLogika.csproj',
  '4_PrezentacioniSloj\PrezentacionaLogika\PrezentacionaLogika\PrezentacionaLogika.csproj'
)
foreach ($p in $projekti) {
  L "Build $p"
  & $msbuild (Join-Path $root $p) /p:Configuration=Debug /v:minimal /nologo
  if ($LASTEXITCODE -ne 0) { throw "Build nije uspeo: $p" }
}
L 'Build OK'

L '=== 4. TESTOVI ==='
$dlls = @(
  (Join-Path $root '1_SlojPodataka\TehnoloskeKlase\DBUtils\DBUtils\DBUtils\bin\Debug\DBUtils.dll'),
  (Join-Path $root '1_SlojPodataka\KlasePodataka\KlasePodataka\KlasePodataka\bin\Debug\KlasePodataka.dll'),
  (Join-Path $root '3_SlojServisa\KlaseMapiranja\KlaseMapiranja\bin\Debug\KlaseMapiranja.dll'),
  (Join-Path $root '2_SlojPoslovneLogike\PoslovnaLogika\PoslovnaLogika\bin\Debug\PoslovnaLogika.dll'),
  (Join-Path $root '4_PrezentacioniSloj\PrezentacionaLogika\PrezentacionaLogika\bin\Debug\PrezentacionaLogika.dll')
)
$refs = ($dlls | ForEach-Object { '/r:' + $_ }) -join ' '
$exe = Join-Path $PSScriptRoot 'Testovi.exe'
$src = Join-Path $PSScriptRoot 'Testovi.cs'
$cfg = Join-Path $PSScriptRoot 'Testovi.exe.config'
Copy-Item $cfg $exe.Replace('.exe','.exe.config') -Force
$xml = Join-Path $root '3_SlojServisa\WebServis\ParametriKvaliteta\ParametriKvaliteta\XML\OgranicenjaKvaliteta.XML'

$argList = @('/nologo', '/t:exe', "/out:$exe", '/r:System.Data.dll', '/r:System.Web.Services.dll', '/r:System.dll', '/r:System.Xml.dll', '/r:System.Configuration.dll', '/r:System.EnterpriseServices.dll') + ($dlls | ForEach-Object { "/r:$_" }) + @($src)
& $csc @argList
if ($LASTEXITCODE -ne 0) { throw 'Kompajliranje testova nije uspelo' }

foreach ($d in $dlls) { Copy-Item $d $PSScriptRoot -Force }
$plCfg = Join-Path $root '2_SlojPoslovneLogike\PoslovnaLogika\PoslovnaLogika\bin\Debug\PoslovnaLogika.dll.config'
if (Test-Path $plCfg) { Copy-Item $plCfg (Join-Path $PSScriptRoot 'PoslovnaLogika.dll.config') -Force }

& $exe $xml
$kod = $LASTEXITCODE
L "Testovi exit=$kod"
exit $kod
