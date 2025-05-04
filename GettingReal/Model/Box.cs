namespace GettingReal.Model;

public class Box : ISaveable<Box>
{
    public int ID { get; set; } 
    public string Name { get; set; } // Navn på kassen f.eks. "Kasse 3"

    public List<Material> Materials { get; set; } // Materialer i kassen

    //Tom Metode Ind Til Videre!
    public Box()
    {
        Materials = new List<Material>(); //Initialiserer tom liste
    }

    public string ToString()
    {
        return $"{ID},{Name}";
    }

    public Box FromString(string line)
    {
        //string[] parts = line.Split(';');
        //if (parts.Length < 2)
        //    throw new ArgumentException("Invalid line format");
        //Box box = new Box
        //{
        //    ID = int.Parse(parts[0]),
        //    Name = parts[1]
        //};
        //// Hvis der er flere dele, kan vi tilføje dem til Materials
        //for (int i = 2; i < parts.Length; i++)
        //{
        //    box.Materials.Add(new Material { UID = parts[i] });
        //}
        return new Box();
    }
}