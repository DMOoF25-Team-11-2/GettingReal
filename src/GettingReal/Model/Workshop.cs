namespace GettingReal.Model;
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

    /// <summary>
    /// Gets or sets the description of the workshop.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the expected time for the workshop.
    /// </summary>
    public List<Guid> ActivityGuids { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Workshop"/> class.
    /// </summary>
    public Workshop() : this(string.Empty, string.Empty)
    {
        GUID = Guid.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Workshop"/> class with the specified name and description.
    /// </summary>
    public Workshop(string name, string description)
    {
        GUID = Guid.NewGuid();
        Name = name;
        Description = description;
        ActivityGuids = [];
        MaterialGuids = [];
    }
}
