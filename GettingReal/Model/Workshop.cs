namespace GettingReal.Model;

/// <summary>
/// Represents a workshop that contains a list of activities.
/// </summary>
public class Workshop
{
    /// <summary>
    /// Gets the unique identifier for the workshop.
    /// </summary>
    public Guid GUID { get; private set; }

    /// <summary>
    /// Gets or sets the name of the workshop.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the list of activities associated with the workshop.
    /// </summary>
    public List<Activity> Activities { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Workshop"/> class.
    /// </summary>
    public Workshop()
    {
        Activities = [];
        Name = "";
    }
}
