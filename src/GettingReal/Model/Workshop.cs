namespace GettingReal.Model;

using System.Xml.Serialization;

/// <summary>
/// Represents a workshop that contains a list of activities.
/// </summary>
public class Workshop
{
    /// <summary>
    /// Gets the unique identifier for the workshop.
    /// </summary>
    public Guid GUID { get; set; }

    /// <summary>
    /// Gets or sets the name of the workshop.
    /// </summary>
    public string Name { get; set; }

    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the list of activities associated with the workshop.
    /// </summary>
    [XmlIgnore]
    public List<Activity> Activities { get; set; }

    [XmlArrayItem("ActivityGUID")]
    public List<Guid> ActivityGUIDs
    {
        get => Activities?.Select(m => m.GUID).ToList() ?? new List<Guid>();
        set
        {
            if (Activities == null)
                Activities = new List<Activity>();
            Activities.Clear();
            if (value != null)
            {
                foreach (var guid in value)
                {
                    Activities.Add(new Activity { GUID = guid });
                }
            }
        }
    }

    [XmlIgnore]
    public List<Material> Materials { get; set; }

    [XmlArrayItem("ActivityGUID")]
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

    /// <summary>
    /// Initializes a new instance of the <see cref="Workshop"/> class.
    /// </summary>
    public Workshop()
    {
        Activities = [];
        Name = string.Empty;
        Description = string.Empty;
    }
    public Workshop(string name, string description)
    {
        Activities = [];
        Name = name;
        Description = description;
    }
}
