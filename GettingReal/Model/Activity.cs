namespace GettingReal.Model;

public class Activity
{
    public string UID { get; set; } 
    public string Name { get; set; }
    public DateTime ExpectedTime { get; set; } //Forventet Tidspunkt For Aktivitet

    public Material? Material { get; set; } //Det materiale, der bruges (kan være null)
}

//Ingen Metoder Endnu!