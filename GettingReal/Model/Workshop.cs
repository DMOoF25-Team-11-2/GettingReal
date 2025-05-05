namespace GettingReal.Model;

public class Workshop : ISaveable<Workshop>
{
    public string UID { get; set; } 
    public string Name { get; set; } 

    //Tom Metode Ind Til Videre!
    public List<Activity> Activities { get; set; } //Aktiviteter i workshoppen

    //Tom Metode Ind Til Videre!
    public Workshop()
    {
        Activities = new List<Activity>(); //Initialiserer tom liste
    }

    public Workshop FromString(string input)
    {
        throw new NotImplementedException();
    }

    public string ToString()
    {
        return $"{UID},{Name}";
    }
}