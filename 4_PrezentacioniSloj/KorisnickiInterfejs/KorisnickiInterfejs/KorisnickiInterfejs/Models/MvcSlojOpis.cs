namespace KorisnickiInterfejs.Models
{
    /// <summary>
    /// ViewModel sloj MVC arhitekture: kontroler popunjava ove klase iz Forma*Klasa
    /// (prezentaciona logika) i predaje ih pogledu.
    /// </summary>
    public class MvcSlojOpis
    {
        public string Model { get { return "KlasePodataka + ViewModel"; } }
        public string View { get { return "Views/*.cshtml"; } }
        public string Controller { get { return "Controllers/*Controller"; } }
    }
}
