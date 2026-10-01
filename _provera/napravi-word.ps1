# Word dokumentacija po strukturi sa Google ucionice (Open XML, bez MS Word-a).
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$outDir = Join-Path $root 'Dokumentacija'
$docx = Join-Path $outDir 'Evidencija-prijema-grozdja-seminarski.docx'
$tmp = Join-Path $env:TEMP ('seminar-docx-' + [guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Force -Path (Join-Path $tmp 'word\_rels') | Out-Null
New-Item -ItemType Directory -Force -Path (Join-Path $tmp '_rels') | Out-Null

function Escape-Xml([string]$t) {
  if ($null -eq $t) { return '' }
  return ($t -replace '&','&amp;' -replace '<','&lt;' -replace '>','&gt;')
}

function P([string]$text, [string]$style = 'Normal') {
  $escaped = Escape-Xml $text
  return @"
<w:p>
  <w:pPr><w:pStyle w:val="$style"/></w:pPr>
  <w:r><w:t xml:space="preserve">$escaped</w:t></w:r>
</w:p>
"@
}

function HeadingXml([int]$n, [string]$text) {
  return (P $text ("Heading$n"))
}

$body = New-Object System.Text.StringBuilder
function A([string]$s, [string]$style = 'Normal') { [void]$body.Append((P $s $style)) }
function Add-Heading([int]$n, [string]$s) { [void]$body.Append((HeadingXml $n $s)) }

A 'Univerzitet u Novom Sadu' 'Title'
A 'Tehnicki fakultet "Mihajlo Pupin" Zrenjanin' 'Subtitle'
A 'Katedra za informacione tehnologije' 'Subtitle'
A 'Skolska 2025/26' 'Subtitle'
A 'EVIDENCIJA PRIJEMA GROZDJA U VINARIJI' 'Title'
A 'Seminarski rad iz predmeta Razvoj viseslojnog softvera' 'Subtitle'
A 'Predmetni nastavnik: Prof. dr Ljubica Kazi' 'Subtitle'
A 'Dokument poslovnog procesa: Zapisnik o prijemu grozdja' 'Subtitle'
A 'Prezime: Škorić    Ime: Luka    Broj indeksa: SI 23/21' 'Subtitle'

Add-Heading 1 '1. Opis namene aplikacije u kontekstu poslovnog procesa i poslovnog pravila'
A 'Aplikacija podrzava poslovni proces evidencije prijema grozdja u vinariji. Na prijemnom mestu se za svaku turu grozdja meri neto masa, sadrzaj secera, kiselost, temperatura i udeo ostecenih bobica. Podaci se upisuju u dokument Zapisnik o prijemu grozdja. Jedan zapisnik (prijem) moze da sadrzi vise tura (odnos celina-deo, master-detail). Sistem ne obradjuje otkup ni cenu, vec merenja kvaliteta i usmerenje prerade.'
A 'Poslovno pravilo je parametrizovano i ima oblik AKO uslov ONDA automatska akcija:'
A 'AKO je sadrzaj secera u turi (%) manji od minimuma X za tu sortu ILI je udeo ostecenih bobica veci od dozvoljenog Y%, ONDA sistem automatski usmerava turu u destilat (ne dozvoljava usmerenje "kvalitetno vino").'
A 'Parametri odlucivanja X i Y nisu u kodu, vec u eksternom XML fajlu OgranicenjaKvaliteta.XML (Rizling: 19% / 10%, Tamjanika: 18% / 10%, Prokupac: 20% / 10%). Poslovna logika ih cita pozivom SOAP servisa, a isti parametri su dostupni i preko REST servisa u JSON formatu.'

Add-Heading 1 '2. Dokument i analiza dokumenta'
A 'Izgled dokumenta Zapisnik o prijemu grozdja: zaglavlje (Broj prijema, Datum, Isporucilac) i tabela stavki (Oznaka, Sorta, Neto kg, Secer %, Kiselost g/l, temperatura C, Udeo ostecenih %, Usmerenje). Primer: PG-2026-0148, 18.09.2026, Petar Jovanovic; tura T-0148-01 Rizling 1820 kg 21,4% -> kvalitetno vino; tura T-0148-02 Rizling 940 kg 16,1% i 12,5% ostecenih -> destilat.'
A 'Tabela elementarnih podataka (naziv, standardni tip, domen):'
A 'Broj prijema - string - nvarchar(20), jedinstven, format PG-GGGG-NNNN.'
A 'Datum prijema - date - date, ispravan kalendarski datum.'
A 'Isporucilac - string - nvarchar(80), nije prazan.'
A 'Oznaka ture - string - nvarchar(10), jedinstvena u tabeli TURA.'
A 'Sifra sorte - string - char(10), FK na SORTA.Sifra.'
A 'Naziv sorte - string - nvarchar(50), vrednost iz sifarnika SORTA.'
A 'Neto masa (kg) - decimal - decimal(10,2), vece od 0.'
A 'Secer (%) - decimal - decimal(5,2), 0-100.'
A 'Kiselost (g/l) - decimal - decimal(5,2), vece ili jednako 0.'
A 'Temperatura (C) - decimal - decimal(5,2).'
A 'Udeo ostecenih (%) - decimal - decimal(5,2), 0-100.'
A 'Usmerenje - string - domen {kvalitetno vino, destilat}.'
A 'Korisnicko ime / sifra - string - nalog iz tabele Korisnik.'
A 'Objasnjenje domena. Broj prijema identifikuje celinu dokumenta. Oznaka ture identifikuje stavku. Sorta mora da postoji u sifarniku. Numericka merenja su decimalni brojevi; secer i udeo ostecenih su procenti. Usmerenje nije slobodan tekst - poslovna logika ga postavlja na jednu od dve vrednosti iz domena, na osnovu XML parametara X i Y. Korisnik je nezavisna tabela za prijavu (admin / admin).'

Add-Heading 1 '3. Korisnicko uputstvo'
A 'Aplikacija se pokrece na http://localhost:1202/Prijava. Jezik interfejsa, baze i URL-ova je srpski. Nema Web Forms stranica (.aspx / .Master) - samo ASP.NET MVC sa ViewModel klasama.'
Add-Heading 2 '3.1 Prijava'
A 'Ekran Prijava korisnika: polja Korisnicko ime i Sifra, dugme PRIJAVA. JavaScript proverava da polja nisu prazna. Ako nalog postoji u tabeli Korisnik, otvara se pocetna strana. Demo nalog: admin / admin.'
Add-Heading 2 '3.2 Pocetna strana'
A 'Zaglavlje Vinarija, meni SORTE i PRIJEM GROZDJA, ODJAVA. Kartice: Unos ture, Spisak tura, Sorte, Stampa zapisnika.'
Add-Heading 2 '3.3 Sifarnik sorti'
A 'Tabelarni prikaz (/Sorta/Tabelarni) - lista sorti. Prikaz svih sorti ide preko Entity Framework-a. Filter po nazivu ide preko stored procedure. Dugmad FILTRIRAJ / SVI. Unos (/Sorta/Unos) - sifra i naziv. Izmena i brisanje - izbor iz tabele pa detalji. Stampa i parametarska stampa - printer-friendly spisak.'
Add-Heading 2 '3.4 Unos ture i spisak'
A 'Na /Tura/Unos se unose merenja jedne ture. Usmerenje se predlaze poslovnim pravilom (SOAP + XML). Na /Tura/Spisak je tabelarni prikaz sa filterom po isporuciocu.'
Add-Heading 2 '3.5 Master-detail unos i prikaz prijema'
A 'Na /Prijem/Unos korisnik popunjava zaglavlje (broj, datum, isporucilac), dodaje ture dugmetom DODAJ TURU u tabelu stavki, zatim SNIMI PRIJEM. Zaglavlje i sve stavke se snimaju u jednoj transakciji. Prikaz prijema: izbor broja prijema, zatim sve ture tog zapisnika.'
Add-Heading 2 '3.6 Stampa'
A 'Stampa zapisnika (/Tura/Stampa) daje spisak svih tura u printer-friendly izgledu. Parametarska stampa trazi isporucioca i stampa filtrirani dokument. Izgled odgovara dokumentu sa prijave teme (zaglavlje + stavke).'

Add-Heading 1 '4. Kratak opis primenjenih alata'
A 'Microsoft Visual Studio 2022 - razvoj, kompajliranje, IIS Express.'
A 'C# / .NET Framework 4.0 (biblioteke) i 4.8 (MVC UI) - programski jezik i runtime.'
A 'ASP.NET MVC 5.2.9 + Razor - prezentacioni sloj: Controller, View, ViewModel.'
A 'Microsoft SQL Server LocalDB - relaciona baza VP2026Vinarija.'
A 'ADO.NET (SqlConnection, SqlCommand, SqlDataAdapter) - SQL client + stored procedure.'
A 'DBUtils (TabelaKlasa) - nasledjivanje i izvrsavanje SQL upita / transakcija.'
A 'Entity Framework 6.1.3 - ORM, treci nacin rada sa bazom (klasa SORTA).'
A 'ASP.NET Web Services (ASMX) + XML - SOAP servis parametara poslovnog pravila.'
A 'IHttpHandler (.ashx) - REST servis (JSON) za parametre i CRUD podatke.'
A 'JavaScript (Validacija.js) - klijentske validacije i regularni izrazi.'
A 'Arhitektura nema ASP.NET Web Forms. Korisnicki interfejs je iskljucivo MVC sa ViewModel klasama.'

Add-Heading 1 '5. Modeli dokumentovanja realizovanog resenja'
Add-Heading 2 '5.1 Dijagram slucajeva koriscenja'
A 'Akter: prijavljeni korisnik. Softverske funkcije: Prijava; CRUD sorte; Unos ture; Spisak tura (tabelarni prikaz sa filterom); Master-detail unos prijema; Master-detail prikaz prijema; Stampa spiska; Parametarska stampa. (Graficki prikaz je u HTML dokumentu, odeljak 5.1.)'
Add-Heading 2 '5.2 Relacioni model podataka'
A 'Baza VP2026Vinarija. SORTA (Sifra PK, Naziv) - sifarnik. PRIJEM (BrojPrijema PK, DatumPrijema, Isporucilac) - zaglavlje dokumenta. TURA (Oznaka PK, BrojPrijema FK, DatumPrijema, Isporucilac, SifraSorte FK, NetoMasaKg, SecerProcenat, KiselostGl, TemperaturaC, UdeoOstecenihProcenat, Usmerenje) - stavke. Korisnik (KorisnickoIme, Sifra, Ime, Prezime) - nezavisna tabela za login. Veze: PRIJEM 1:N TURA, SORTA 1:N TURA.'
Add-Heading 2 '5.3 Dijagram komponenti'
A 'Prezentacioni sloj MVC: Views (Razor), Controllers, ViewModel, Forma*Klasa. Sloj servisa: OgranicenjaKvaliteta.asmx (SOAP), OgranicenjaRest.ashx i PodaciRest.ashx (REST JSON), XML OgranicenjaKvaliteta.XML. Sloj poslovne logike: UsmerenjePreradeKlasa, AnalizaTureKlasa, ParametriKvalitetaKlasa. Sloj podataka: Repository, SP*DBKlasa, TuraDBKlasa/PrijemDBKlasa : TabelaKlasa, VinarijaKontekstKlasa : DbContext, baza VP2026Vinarija.'
Add-Heading 2 '5.4 Dijagram klasa (sloj podataka i poslovna logika)'
A 'Entiteti: SortaKlasa (Sifra, Naziv); TuraKlasa (Oznaka, BrojPrijema, DatumPrijema, Isporucilac, Sorta, merenja, Usmerenje); PrijemKlasa (BrojPrijema, DatumPrijema, Isporucilac, lista Tura). Veza TuraKlasa -> SortaKlasa je asocijacija; PrijemKlasa o-- TuraKlasa je kompozicija.'
A 'Nasledjivanje TabelaKlasa: TuraDBKlasa i PrijemDBKlasa (metode DajPodatke, IzvrsiAzuriranje, SnimiPrijemSaTurama).'
A 'SQL client + SP: SPSortaDBKlasa (DajSveSorte, DajSortePoNazivu, SnimiNovuSortu).'
A 'Repository: SortaRepositoryKlasa, TuraRepositoryKlasa, PrijemRepositoryKlasa (DajSve*, SnimiNovu*, DajSveSortePrekoEntityFramework, DajSveTurePrekoNasledjeneKlase).'
A 'Entity Framework: SortaEfKlasa (Sifra, Naziv); VinarijaKontekstKlasa : DbContext (DbSet Sorte); SortaEfRepositoryKlasa (DajSve, DajPoId, SnimiNovi).'
A 'Poslovna logika: UsmerenjePreradeKlasa.OdrediUsmerenje / DaLiMozeKvalitetnoVino; AnalizaTureKlasa (SecerProcenat, UdeoOstecenihProcenat); ParametriKvalitetaKlasa.DajMinSecerZaSortu / DajMaxUdeoOstecenihZaSortu. UsmerenjePrerade poziva Analizu, Parametre i MaperKlasa.'
Add-Heading 2 '5.5 Dijagram sekvenci'
A 'Unos prijema: Korisnik -> View Unos -> PrijemController POST -> PrijemUnosViewModel -> FormaPrijemUnosKlasa.SnimiPrijemSaTabelom -> za svaku turu UsmerenjePreradeKlasa.OdrediUsmerenje -> PrijemRepositoryKlasa.SnimiPrijemSaTurama -> PrijemDBKlasa.IzvrsiAzuriranje(lista INSERT) -> uspeh transakcije.'
A 'Tabelarni prikaz sorti: Korisnik -> GET /Sorta/Tabelarni -> SortaController -> FormaSortaTabelaEditKlasa.DajPodatkeZaGrid (prazan filter) -> SortaRepositoryKlasa.DajSveSortePrekoEntityFramework -> SortaEfRepositoryKlasa.DajSve -> VinarijaKontekstKlasa.Sorte.ToList -> View tabela.'

Add-Heading 1 '6. Kljucni delovi koda sa objasnjenjem'
Add-Heading 2 '6.1 Realizacija korisnickih softverskih funkcija'
Add-Heading 3 '6.1.1 Prijava korisnika'
A 'Ruta /Prijava otvara PrijavaController.Index. GET vraca prazan PrijavaViewModel. POST mapira ViewModel na FormaLoginKlasa, koja preko SPKorisnikDBKlasa (stored procedure) proverava nalog. Uspeh: sesija KorisnikImePrezime i redirekcija na /Pocetna. Filter ZahtevaPrijavu stiti ostale kontrolere.'
A 'Kod: forma.KorisnickoIme = model.KorisnickoIme; forma.Sifra = model.Sifra; if (forma.VazeciKorisnik()) { Session["KorisnikImePrezime"] = forma.DajImePrezimeKorisnika(); return RedirectToAction("Index", "Pocetna"); }'
Add-Heading 3 '6.1.2 CRUD operacije nad glavnom tabelom'
A 'Glavna tabela dokumenta je TURA (stavke) uz zaglavlje PRIJEM. TuraController: Unos, Spisak (filter), stampa. PrijemController.Unos: unos celine sa delovima na jednoj formi (stavke u sesiji, zatim transakcija). PrijemController.Prikaz: pojedinacni zapis celine sa svim delovima. Sifarnik SORTA ima pun CRUD u SortaController (Tabelarni, Unos, Izmena, Detalji, Obrisi).'
Add-Heading 3 '6.1.3 Stampa i parametarska stampa'
A 'Pogledi koriste layout _Stampa.cshtml (printer-friendly). /Tura/Stampa?filter=0 - spisak svih tura. /Tura/ParametarStampe - unos isporucioca, zatim filtrirani zapisnik. Izgled tabele odgovara dokumentu sa prijave teme.'
Add-Heading 2 '6.2 Realizacija osnovnih validacija'
A 'Na prezentacionoj logici: DaLiJeSvePopunjeno (obavezna polja), provera tipa (decimal.Parse), duzina (maxlength u View), domen (drop-down sorte i usmerenje), jedinstvenost (PK u bazi). Zaglavlje prijema mora imati broj, datum i isporucioca pre snimanja.'
Add-Heading 2 '6.3 Prezentacioni sloj - realizacija MVC i View modela'
A 'Nema Web Forms. Tok: View (Razor) <-> Controller <-> ViewModel <-> Forma*Klasa <-> repository / poslovna logika.'
A 'PrijavaController + Views/Prijava/Index.cshtml + PrijavaViewModel (KorisnickoIme, Sifra, Poruka).'
A 'SortaController + Views/Sorta/* + SortaViewModel (Sifra, Naziv, Filter).'
A 'TuraController + Views/Tura/* + TuraUnosViewModel.'
A 'PrijemController + Views/Prijem/* + PrijemUnosViewModel.'
A 'Rute: podrazumevani kontroler je Prijava. View vezuje polja Html.TextBoxFor(m => m.KorisnickoIme).'
Add-Heading 2 '6.4 Sloj poslovne logike - realizacija poslovnog pravila'
A 'UsmerenjePreradeKlasa.OdrediUsmerenje mapira naziv sorte na sifru servisa (R/T/P), zatim SOAP metodama cita X i Y iz XML-a. Ako je secer < X ili udeo ostecenih > Y, rezultat je destilat, inace kvalitetno vino. XML: Rizling MinSecer=19, MaksimalniUdeoOstecenih=10.'
Add-Heading 2 '6.5 Sloj servisa - realizacija REST servisa'
A 'OgranicenjaRest.ashx?akcija=DajMinSecer&sifra=R - parametri poslovnog pravila iz XML-a, odgovor JSON. PodaciRest.ashx?akcija=DajSveTure - medjusloj prezentacije prema podacima (citanje tura i prijema). Primer: json = "{\"minSecer\":" + minSecer.ToString(...) + "}".'
Add-Heading 2 '6.6 Sloj za rad sa podacima'
Add-Heading 3 '6.6.1 Repository patern'
A 'SortaRepositoryKlasa, TuraRepositoryKlasa, PrijemRepositoryKlasa - metode na srpskom: DajSveSorte, DajPoId, SnimiNovuSortu, DajSveTure, SnimiPrijemSaTurama. Gornji slojevi ne znaju da li se iza repository-ja koristi SP, TabelaKlasa ili EF.'
Add-Heading 3 '6.6.2 Rad sa stored procedurama'
A 'SPSortaDBKlasa: SqlCommand("DajSveSorte"); CommandType = StoredProcedure; SqlDataAdapter.Fill(DataSet).'
Add-Heading 3 '6.6.3 Rad sa DBUtils i primena upita'
A 'TuraDBKlasa i PrijemDBKlasa nasledjuju TabelaKlasa. public DataSet DajSvePrijeme() { return this.DajPodatke("select * from PRIJEM"); } Transakcioni unos vise naredbi: IzvrsiAzuriranje(List<string>).'
Add-Heading 3 '6.6.4 Rad sa Entity Framework klasama'
A 'Entitet SortaEfKlasa (Sifra, Naziv). Kontekst VinarijaKontekstKlasa nasledjuje DbContext, Database.SetInitializer(null), DbSet Sorte, mapiranje tabele SORTA u OnModelCreating. SortaEfRepositoryKlasa: kontekst.Sorte.ToList() i SaveChanges() za snimanje. Tabelarni prikaz sorti bez filtera koristi ovaj put.'
Add-Heading 2 '6.7 Opciono - Master-detail unos sa transakcijom i tabelarni prikaz'
A 'Na formi prijema korisnik dodaje vise tura u tabelu u sesiji, zatim snima celinu. PrijemDBKlasa.SnimiPrijemSaTurama pravi listu INSERT naredbi (jedna za PRIJEM, po jedna za svaku TURU) i poziva TabelaKlasa.IzvrsiAzuriranje(listaUpita) - sve ili nista. Tabelarni prikaz: spisak tura i prikaz svih stavki izabranog prijema.'
Add-Heading 2 '6.8 Opciono - JavaScript validacije i regularni izrazi'
A 'Fajl Scripts/Validacija.js: provera praznih polja, numerickih vrednosti, izbora iz domena. Regularni izraz za oznaku ture: ^[A-Za-z0-9\-]{1,10}$. Regularni izraz za broj prijema: ^[A-Za-z0-9\-]{1,20}$. Funkcije se vezuju na onsubmit / onclick u Razor pogledima.'

$documentXml = @"
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<w:document xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main">
  <w:body>
$($body.ToString())
    <w:sectPr>
      <w:pgSz w:w="11906" w:h="16838"/>
      <w:pgMar w:top="1440" w:right="1440" w:bottom="1440" w:left="1440"/>
    </w:sectPr>
  </w:body>
</w:document>
"@

$stylesXml = @'
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<w:styles xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main">
  <w:style w:type="paragraph" w:default="1" w:styleId="Normal">
    <w:name w:val="Normal"/>
    <w:rPr><w:rFonts w:ascii="Times New Roman" w:hAnsi="Times New Roman"/><w:sz w:val="24"/></w:rPr>
  </w:style>
  <w:style w:type="paragraph" w:styleId="Title">
    <w:name w:val="Title"/>
    <w:basedOn w:val="Normal"/>
    <w:pPr><w:jc w:val="center"/><w:spacing w:after="200"/></w:pPr>
    <w:rPr><w:b/><w:sz w:val="36"/></w:rPr>
  </w:style>
  <w:style w:type="paragraph" w:styleId="Subtitle">
    <w:name w:val="Subtitle"/>
    <w:basedOn w:val="Normal"/>
    <w:pPr><w:jc w:val="center"/><w:spacing w:after="80"/></w:pPr>
    <w:rPr><w:sz w:val="24"/></w:rPr>
  </w:style>
  <w:style w:type="paragraph" w:styleId="Heading1">
    <w:name w:val="heading 1"/>
    <w:basedOn w:val="Normal"/>
    <w:pPr><w:spacing w:before="360" w:after="160"/></w:pPr>
    <w:rPr><w:b/><w:sz w:val="28"/><w:color w:val="5C1A2E"/></w:rPr>
  </w:style>
  <w:style w:type="paragraph" w:styleId="Heading2">
    <w:name w:val="heading 2"/>
    <w:basedOn w:val="Normal"/>
    <w:pPr><w:spacing w:before="280" w:after="120"/></w:pPr>
    <w:rPr><w:b/><w:sz w:val="26"/><w:color w:val="5C1A2E"/></w:rPr>
  </w:style>
  <w:style w:type="paragraph" w:styleId="Heading3">
    <w:name w:val="heading 3"/>
    <w:basedOn w:val="Normal"/>
    <w:pPr><w:spacing w:before="200" w:after="80"/></w:pPr>
    <w:rPr><w:b/><w:sz w:val="24"/><w:color w:val="7A2A42"/></w:rPr>
  </w:style>
</w:styles>
'@

$contentTypes = @'
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
  <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>
  <Default Extension="xml" ContentType="application/xml"/>
  <Override PartName="/word/document.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml"/>
  <Override PartName="/word/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.styles+xml"/>
</Types>
'@

$rels = @'
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
  <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="word/document.xml"/>
</Relationships>
'@

$docRels = @'
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
  <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles" Target="styles.xml"/>
</Relationships>
'@

$utf8 = New-Object System.Text.UTF8Encoding $true
[System.IO.File]::WriteAllText((Join-Path $tmp '[Content_Types].xml'), $contentTypes, $utf8)
[System.IO.File]::WriteAllText((Join-Path $tmp '_rels\.rels'), $rels, $utf8)
[System.IO.File]::WriteAllText((Join-Path $tmp 'word\document.xml'), $documentXml, $utf8)
[System.IO.File]::WriteAllText((Join-Path $tmp 'word\styles.xml'), $stylesXml, $utf8)
[System.IO.File]::WriteAllText((Join-Path $tmp 'word\_rels\document.xml.rels'), $docRels, $utf8)

if (Test-Path $docx) { Remove-Item $docx -Force }
Add-Type -AssemblyName System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::CreateFromDirectory($tmp, $docx, [System.IO.Compression.CompressionLevel]::Optimal, $false)
Remove-Item $tmp -Recurse -Force
Write-Output "Napravljen: $docx"
