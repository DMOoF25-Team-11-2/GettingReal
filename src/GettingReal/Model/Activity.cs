namespace GettingReal.Model;

/// <summary>
/// Represents an activity with a unique identifier, name, description, expected time, and associated materials.
/// </summary>
public class Activity
{
    /// <summary>
    /// Gets or sets the unique identifier for the activity.
    /// </summary>
    public Guid GUID { get; set; }

    /// <summary>
    /// Gets or sets the name of the activity.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the activity.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the expected time for the activity.
    /// </summary>
    public TimeSpan ExpectedTime { get; set; }

    /// <summary>
    /// Gets or sets the list of material GUIDs associated with the activity.
    /// </summary>
    public List<Guid> MaterialGUIDs { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Activity"/> class with default values.
    /// </summary>
    public Activity() : this(string.Empty, TimeSpan.Zero, string.Empty)
    {
        GUID = Guid.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Activity"/> class with the specified name, expected time, and description.
    /// </summary>
    /// <param name="name">The name of the activity.</param>
    /// <param name="expectedTime">The expected time for the activity.</param>
    /// <param name="description">The description of the activity.</param>
    public Activity(string name, TimeSpan expectedTime, String description)
    {
        GUID = Guid.NewGuid();
        Name = name;
        ExpectedTime = expectedTime;
        Description = description;
        MaterialGUIDs = [];
    }
}