namespace GettingReal.Model;

public class Material
{
    public Guid GUID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    [Obsolete("Use Quantity instead", true)]
    public int Amount { get => Quantity; set { Quantity = value; } }

    public int Quantity { get; set; }

    // Add a public parameterless constructor to satisfy the constraint in RepositoryBase<T>
    public Material()
    {
        GUID = Guid.NewGuid();
        Name = string.Empty;
        Description = string.Empty;
        Quantity = 0;
    }

    public Material(string name, string description, int quantity)
    {
        GUID = Guid.NewGuid();
        this.Name = name;
        this.Description = description;
        this.Quantity = quantity;
    }
}
