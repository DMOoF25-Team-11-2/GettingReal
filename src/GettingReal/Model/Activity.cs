using System.Xml.Serialization;

namespace GettingReal.Model;

public class Activity
{
    public Guid GUID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public TimeSpan ExpectedTime { get; set; }
    [XmlIgnore]
    public ICollection<Material>? Materials { get; set; }
    [XmlArrayItem("MaterialGUID")]
    public List<Guid> MaterialGUIDs
    {
        get => Materials?.Select(m => m.GUID).ToList() ?? new List<Guid>();
        set
        {
            if (Materials == null)
                Materials = new List<Material>();
            Materials.Clear();
            if (value != null)
            {
                foreach (var guid in value)
                {
                    Materials.Add(new Material { GUID = guid });
                }
            }
        }
    }

    public Activity()
    {
        GUID = Guid.NewGuid();
        Name = string.Empty;
        ExpectedTime = TimeSpan.Zero;
        Materials = new List<Material>();
    }

    public Activity(string name, TimeSpan expectedTime, String description)
    {
        GUID = Guid.NewGuid();
        Name = name;
        ExpectedTime = expectedTime;
        Description = description;
        Materials = new List<Material>();
    }
}