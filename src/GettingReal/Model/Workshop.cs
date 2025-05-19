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
    }

    public IEnumerable<Activity> GetActivitiesForWorkshop()
    {
        List<Activity> result = [];
        if (ActivityGuids != null && ActivityGuids.Count != 0)
        {
            IEnumerable<Activity> activityList = new ActivityRepository().GetAll();
            for (int i = 0; i < activityList.Count(); i++)
            {
                var activity = activityList.ElementAt(i);
                if (activity == null)
                    continue;
                if (ActivityGuids.Contains(activity.GUID))
                {
                    result.Add(activity);
                }
            }
        }
        return result;
    }

    public IEnumerable<Box> GetBoxesForWorkshop()
    {
        // 1. Get all activities for this workshop
        var activities = GetActivitiesForWorkshop();
        if (activities == null || !activities.Any())
            return [];

        // 2. Collect all material GUIDs from all activities
        var requiredMaterialGuids = new HashSet<Guid>(
            activities
                .Where(a => a.MaterialGuids != null)
                .SelectMany(a => a.MaterialGuids)
        );

        // 3. Get all boxes
        var allBoxes = new BoxRepository().GetAll();

        // 4. Find boxes that contain any of the required materials
        var result = allBoxes
            .Where(box => box.MaterialGuids != null && box.MaterialGuids.Any(guid => requiredMaterialGuids.Contains(guid)))
            .ToList();

        // 5. Find missing material GUIDs
        var foundMaterialGuids = new HashSet<Guid>(
            result
                .Where(box => box.MaterialGuids != null)
                .SelectMany(box => box.MaterialGuids)
                .Where(guid => requiredMaterialGuids.Contains(guid))
        );

        var missingMaterialGuids = requiredMaterialGuids.Except(foundMaterialGuids).ToList();

        if (missingMaterialGuids.Count != 0)
        {
            foreach (var guid in missingMaterialGuids)
            {
                var material = new MaterialRepository().Get(guid);
                if (material != null)
                {
                    var box = new Box("No box", material.Name);
                    result.Add(box);
                }
            }
        }

        return result;
    }

}
