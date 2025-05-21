namespace GettingReal.Model;

/// <summary>
/// Represents a material with a unique identifier, name, description, and quantity.
/// </summary>
public class Material
{
    /// <summary>
    /// Gets or sets the unique identifier for the material.
    /// </summary>
    public Guid GUID { get; set; }

    /// <summary>
    /// Gets or sets the name of the material.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the material.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the material.
    /// </summary>
    /// <remarks>
    /// This property is marked as obsolete. Use the <see cref="Quantity"/> property instead.
    /// </remarks>
    [Obsolete("Use Quantity instead", true)]
    public int Amount { get => Quantity; set { Quantity = value; } }

    /// <summary>
    /// Gets or sets the quantity of the material.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Material"/> class with default values.
    /// </summary>
    public Material() : this(string.Empty, string.Empty, 0)
    {
        GUID = Guid.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Material"/> class with the specified name, description, and quantity.
    /// </summary>
    public Material(string name, string description, int quantity)
    {
        GUID = Guid.NewGuid();
        Name = name;
        Description = description;
        Quantity = quantity;
    }

    /// <summary>
    /// Gets the box that contains this material.
    /// </summary>
    /// <returns>
    /// The box that contains this material, or null if no box contains it.
    /// </returns>
    /// <remarks>
    /// This method retrieves all boxes from the repository and checks if this material's GUID is present in any of the boxes' MaterialGuids.
    /// </remarks>
    public Box? GetBoxForMaterial()
    {
        List<Box> boxes = new BoxRepository().GetAll();
        for (int i = 0; i < boxes.Count; i++)
        {
            var materialGuids = boxes[i].MaterialGuids;
            if (materialGuids != null && materialGuids.Contains(GUID))
                return boxes[i];
        }
        return null;
    }
}
