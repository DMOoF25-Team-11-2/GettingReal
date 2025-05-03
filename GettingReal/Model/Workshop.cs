namespace GettingReal.Model;

public class Workshop
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
}