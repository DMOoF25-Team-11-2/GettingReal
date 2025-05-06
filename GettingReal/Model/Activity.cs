namespace GettingReal.Model;

public class Activity
{
    public Guid GUID { get; private set; }
    public string Name { get; set; }
    public DateTime ExpectedTime { get; set; }
    public ICollection<Material>? Material { get; set; }
}