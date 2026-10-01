function TrimTekst(vrednost) {
    return (vrednost || "").replace(/^\s+|\s+$/g, "");
}

function JeBroj(vrednost) {
    var normalizovano = String(vrednost).replace(",", ".");
    if (TrimTekst(normalizovano) === "") {
        return false;
    }
    return !isNaN(parseFloat(normalizovano));
}

function OdgovaraOznaci(vrednost) {
    return /^[A-Za-z0-9\-]{1,10}$/.test(TrimTekst(vrednost));
}

function OdgovaraBrojuPrijema(vrednost) {
    return /^[A-Za-z0-9\-]{1,20}$/.test(TrimTekst(vrednost));
}

function ProveriPrijavu() {
    var ime = document.getElementById("KorisnickoImeTextBox");
    var sifra = document.getElementById("SifraTextBox");
    if (!ime || TrimTekst(ime.value) === "") {
        alert("Unesite korisnicko ime.");
        return false;
    }
    if (!sifra || TrimTekst(sifra.value) === "") {
        alert("Unesite sifru.");
        return false;
    }
    return true;
}

function ProveriUnosSorte() {
    var sifra = document.getElementById("SifraTextBox");
    var naziv = document.getElementById("NazivTextBox");
    if (!sifra || TrimTekst(sifra.value) === "") {
        alert("Unesite sifru sorte.");
        return false;
    }
    if (!naziv || TrimTekst(naziv.value) === "") {
        alert("Unesite naziv sorte.");
        return false;
    }
    return true;
}

function ProveriUnosTure() {
    var polja = [
        { id: "OznakaTextBox", poruka: "Unesite oznaku ture." },
        { id: "BrojPrijemaTextBox", poruka: "Unesite broj prijema." },
        { id: "DatumPrijemaTextBox", poruka: "Unesite datum prijema." },
        { id: "IsporucilacTextBox", poruka: "Unesite isporucioca." },
        { id: "NetoMasaKgTextBox", poruka: "Unesite neto masu." },
        { id: "SecerProcenatTextBox", poruka: "Unesite sadrzaj secera." },
        { id: "KiselostGlTextBox", poruka: "Unesite kiselost." },
        { id: "TemperaturaCTextBox", poruka: "Unesite temperaturu." },
        { id: "UdeoOstecenihProcenatTextBox", poruka: "Unesite udeo ostecenih bobica." }
    ];
    for (var i = 0; i < polja.length; i++) {
        var el = document.getElementById(polja[i].id);
        if (!el || TrimTekst(el.value) === "") {
            alert(polja[i].poruka);
            if (el) { el.focus(); }
            return false;
        }
    }
    if (!OdgovaraOznaci(document.getElementById("OznakaTextBox").value)) {
        alert("Oznaka ture sme da sadrzi samo slova, cifre i crticu (do 10 karaktera).");
        return false;
    }
    if (!OdgovaraBrojuPrijema(document.getElementById("BrojPrijemaTextBox").value)) {
        alert("Broj prijema sme da sadrzi samo slova, cifre i crticu (do 20 karaktera).");
        return false;
    }
    var sorta = document.getElementById("SortaDropDownList");
    if (!sorta || sorta.value === "Izaberite...") {
        alert("Izaberite sortu.");
        return false;
    }
    var brojevna = ["NetoMasaKgTextBox", "SecerProcenatTextBox", "KiselostGlTextBox", "TemperaturaCTextBox", "UdeoOstecenihProcenatTextBox"];
    for (var j = 0; j < brojevna.length; j++) {
        var b = document.getElementById(brojevna[j]);
        if (!JeBroj(b.value)) {
            alert("Vrednost mora biti broj.");
            b.focus();
            return false;
        }
    }
    return true;
}

function ProveriZaglavljePrijema() {
    var broj = document.getElementById("BrojPrijemaTextBox");
    var datum = document.getElementById("DatumPrijemaTextBox");
    var isporucilac = document.getElementById("IsporucilacTextBox");
    if (!broj || TrimTekst(broj.value) === "") {
        alert("Unesite broj prijema.");
        return false;
    }
    if (!datum || TrimTekst(datum.value) === "") {
        alert("Unesite datum prijema.");
        return false;
    }
    if (!isporucilac || TrimTekst(isporucilac.value) === "") {
        alert("Unesite isporucioca.");
        return false;
    }
    return true;
}

function ProveriStavkuTure() {
    if (ProveriZaglavljePrijema() === false) {
        return false;
    }
    var oznaka = document.getElementById("OznakaTextBox");
    var sorta = document.getElementById("SortaDropDownList");
    if (!oznaka || TrimTekst(oznaka.value) === "") {
        alert("Unesite oznaku ture.");
        return false;
    }
    if (!OdgovaraOznaci(oznaka.value)) {
        alert("Oznaka ture sme da sadrzi samo slova, cifre i crticu (do 10 karaktera).");
        return false;
    }
    if (!sorta || sorta.value === "Izaberite...") {
        alert("Izaberite sortu.");
        return false;
    }
    var brojevna = ["NetoMasaKgTextBox", "SecerProcenatTextBox", "KiselostGlTextBox", "TemperaturaCTextBox", "UdeoOstecenihProcenatTextBox"];
    for (var i = 0; i < brojevna.length; i++) {
        var el = document.getElementById(brojevna[i]);
        if (!el || TrimTekst(el.value) === "" || !JeBroj(el.value)) {
            alert("Sva merenja ture moraju biti popunjena brojevima.");
            if (el) { el.focus(); }
            return false;
        }
    }
    return true;
}
