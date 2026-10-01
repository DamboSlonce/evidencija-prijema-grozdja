# Pokrece LocalDB + SOAP servis (1718) + Web Forms (1202)
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent

function Find-IisExpress {
  foreach ($p in @(
      "${env:ProgramFiles}\IIS Express\iisexpress.exe",
      "${env:ProgramFiles(x86)}\IIS Express\iisexpress.exe"
    )) {
    if (Test-Path $p) { return $p }
  }
  return $null
}

sqllocaldb start MSSQLLocalDB | Out-Null

$iis = Find-IisExpress
if (-not $iis) {
  throw 'IIS Express nije pronadjen. Sacekaj da se zavrsi instalacija Visual Studio (ASP.NET workload).'
}

$ws = Join-Path $root '3_SlojServisa\WebServis\ParametriKvaliteta\ParametriKvaliteta'
$ui = Join-Path $root '4_PrezentacioniSloj\KorisnickiInterfejs\KorisnickiInterfejs\KorisnickiInterfejs'

Write-Output "SOAP:  http://localhost:1718/OgranicenjaKvaliteta.asmx"
Write-Output "UI:    http://localhost:1202/Prijava"
Write-Output "Prijava: admin / admin"
Write-Output "Zatvori ovaj prozor da ugasis oba sajta."

$p1 = Start-Process -FilePath $iis -ArgumentList @("/path:`"$ws`"", '/port:1718', '/clr:v4.0') -PassThru
Start-Sleep -Seconds 2
$p2 = Start-Process -FilePath $iis -ArgumentList @("/path:`"$ui`"", '/port:1202', '/clr:v4.0') -PassThru

Start-Process 'http://localhost:1202/Prijava'
Wait-Process -Id $p1.Id, $p2.Id
