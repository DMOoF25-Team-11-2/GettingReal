namespace GettingReal.Model;

public class Activity : ISaveable<Activity>
{
    public string UID { get; set; } 
    public string Name { get; set; }
    public DateTime ExpectedTime { get; set; } //Forventet Tidspunkt For Aktivitet

    public Material? Material { get; set; } //Det materiale, der bruges (kan være null)

    public Activity FromString(string input)
    {
        throw new NotImplementedException();
    }

    public string ToString()
    {
        return $"{UID},{Name},{ExpectedTime}";
    }
}

//Ingen Metoder Endnu!