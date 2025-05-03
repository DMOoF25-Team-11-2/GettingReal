namespace GettingReal.Model;

public class Box
{
    public int ID { get; set; } 
    public string Name { get; set; } // Navn på kassen f.eks. "Kasse 3"

    public List<Material> Materials { get; set; } // Materialer i kassen

    //Tom Metode Ind Til Videre!
    public Box()
    {
        Materials = new List<Material>(); //Initialiserer tom liste
    }
}