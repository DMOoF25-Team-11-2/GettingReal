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
}
