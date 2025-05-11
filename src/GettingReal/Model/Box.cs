namespace GettingReal.Model;
using System.Xml.Serialization;

/// <summary>
/// Represents a box that can contain materials.
/// </summary>
public class Box
{
    /// <summary>
    /// Unique identifier for the box.
    /// </summary>
    public Guid GUID { get; set; }

    /// <summary>
    /// Name of the box.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description of the box.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// List of materials contained in the box.
    /// </summary>
    /// <remarks>
    /// This property is used to store the materials that are contained in the box.
    /// </remarks>
    [XmlIgnore]
    public List<Material> Materials { get; set; }

    /// <summary>
    /// List of GUIDs representing the materials in the box.
    /// </summary>
    /// <remarks>
    /// This property is used for XML serialization and deserialization.
    /// It converts the list of Material objects to a list of GUIDs.
    /// </remarks>
    [XmlArrayItem("MaterialGUID")]
    public List<Guid> MaterialGUIDs
    {
        get => Materials?.Select(m => m.GUID).ToList() ?? new List<Guid>();
        set
        {
            Materials ??= [];
            Materials.Clear();
            if (value != null)
                foreach (var guid in value)
                    Materials.Add(new Material { GUID = guid });
        }
    }

    /// <summary>
    /// Default constructor for XML serialization.
    /// </summary>
    /// <remarks>
    /// Needs to be public to satisfy the XML serialization requirements.
    /// </remarks>
    public Box()
    {
        GUID = Guid.Empty;
        Name = string.Empty;
        Description = string.Empty;
        Materials = [];
    }

    /// <summary>
    /// Constructor to create a Box with a name and description.
    /// </summary>
    /// <param name="name">The name of the box.</param>
    /// <param name="description">The description of the box.</param>
    /// <remarks>
    /// This constructor is used to create a new Box instance with a specified name and description.
    /// </remarks>
    public Box(string name = "", string description = "") : base()
    {
        GUID = Guid.NewGuid();
        Name = name;
        Description = description;
        Materials = [];
    }
}
