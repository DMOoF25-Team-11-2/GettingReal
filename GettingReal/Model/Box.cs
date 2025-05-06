namespace GettingReal.Model;

public class Box
{
    public Guid GUID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    private List<Material> Materials { get; set; }

    public Box()
    {
        GUID = Guid.NewGuid();
        Materials = new List<Material>(); // Initialize Materials collection
    }

    // Adding a public parameterless constructor to fix CS0310
    public Box(string name = "", string description = "")
    {
        Materials = new List<Material>(); // Initialize Materials collection
        Name = name;
        Description = description;
    }
}
