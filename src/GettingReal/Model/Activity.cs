namespace GettingReal.Model;

public class Activity
{
    public Guid GUID { get; set; }
    public string Name { get; set; }
    public TimeSpan ExpectedTime { get; set; }
    public ICollection<Material>? Material { get; set; }

    public Activity()
    {
        GUID = Guid.NewGuid();
        Name = string.Empty;
        ExpectedTime = TimeSpan.Zero;
        Material = new List<Material>();
    }

    public Activity(string name, TimeSpan expectedTime)
    {
        GUID = Guid.NewGuid();
        Name = name;
        ExpectedTime = expectedTime;
        Material = new List<Material>();
    }
}