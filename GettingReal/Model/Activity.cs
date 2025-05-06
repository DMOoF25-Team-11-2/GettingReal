namespace GettingReal.Model;

public class Activity
{
    public Guid GUID { get; private set; }
    public string Name { get; set; }
    public DateTime ExpectedTime { get; set; }
    public ICollection<Material>? Material { get; set; }

    public Activity()
    {
        GUID = Guid.NewGuid();
        Name = string.Empty;
        ExpectedTime = DateTime.Now;
        Material = new List<Material>();
    }

    public Activity(string name, DateTime expectedTime)
    {
        GUID = Guid.NewGuid();
        Name = name;
        ExpectedTime = expectedTime;
        Material = new List<Material>();
    }
}