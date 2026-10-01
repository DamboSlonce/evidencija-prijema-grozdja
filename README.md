# Evidencija prijema grožđa u vinariji

Seminarski rad — evidencija prijema grožđa  
Dokument: Zapisnik o prijemu grožđa  
Baza: `VP2026Vinarija` na LocalDB (`(localdb)\MSSQLLocalDB`)

## Šta radi na ovom računaru (setup)

Jedna komanda iz korena projekta (pravi bazu, kompajlira slojeve, pokreće 21 test):

```powershell
powershell -ExecutionPolicy Bypass -File "_provera\pokreni-sve.ps1"
```

Prijava: `admin` / `admin`

Poslovno pravilo: ako je šećer (%) ispod minimuma za sortu **ili** udeo oštećenih bobica iznad limita (SOAP/XML), tura ide u **destilat**.

## Visual Studio — zašto F5 ne radi

Na ovom PC-u **nije instaliran Visual Studio**. LocalDB jeste (SQL Server 2019 Express LocalDB), zato baza radi, a Web Forms sajt ne može da se pokrene dok ne instaliraš VS.

1. Preuzmi **Visual Studio 2022 Community** (besplatno):  
   https://visualstudio.microsoft.com/vs/community/
2. U installeru obavezno čekiraj:
   - workload **ASP.NET and web development**
   - individual component **.NET Framework 4.8 targeting pack**
3. Posle instalacije pokreni `_provera\pokreni-sve.ps1` pa u VS:
   - prvo `3_SlojServisa\WebServis\ParametriKvaliteta\ParametriKvaliteta.sln` → **F5** (port **1718**)
   - zatim `4_PrezentacioniSloj\KorisnickiInterfejs\KorisnickiInterfejs\KorisnickiInterfejs.sln` → **F5**
   - start stranica: `/Prijava` (MVC, srpski URL; nije `Account/Login`)

Ako VS pita da upgrade-uje stari .sln (šablon je VS 2010) — prihvati.

## Ručno kreiranje baze (ako ne koristiš skriptu)

```powershell
powershell -ExecutionPolicy Bypass -File "_provera\pokreni-sve.ps1"
```

Konekcija (već u `Web.config`):

`Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=VP2026Vinarija; Integrated Security=True;`
