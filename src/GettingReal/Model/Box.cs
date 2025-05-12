namespace GettingReal.Model;
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
    /// Represents by materials guids.
    /// </summary>
    public List<Guid> MaterialGuids { get; set; }

    /// <summary>
    /// Default constructor for XML serialization.
    /// </summary>
    /// <remarks>
    /// Needs to be public to satisfy the XML serialization requirements.
    /// </remarks>
    public Box() : this("", "")
    {
        GUID = Guid.Empty;
        //Name = string.Empty;
        //Description = string.Empty;
        //Materials = [];
        //MaterialGUIDs = [];
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
        MaterialGuids = [];
    }
}
