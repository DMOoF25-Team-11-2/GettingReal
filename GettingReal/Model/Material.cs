namespace GettingReal.Model;

public class Material : ISaveable<Material>
{
    public string UID { get; set; } 
    public string Name { get; set; } 
    public string Description { get; set; }
    public string Number { get; set; } //Varenummer eller intern kode

    public Box? Box { get; set; } //Den kasse, materialet ligger i (kan være null)

    public Material FromString(string input)
    {
        throw new NotImplementedException();
    }

    public string ToString()
    {
        return $"{UID},{Name},{Description},{Number}";
    }
}

//Ingen Metoder Endnu!