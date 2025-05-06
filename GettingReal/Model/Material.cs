namespace GettingReal.Model;

public class Material
{
    public Guid GUID { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Amount { get; set; }

    // Add a public parameterless constructor to satisfy the constraint in RepositoryBase<T>
    public Material()
    {
        GUID = Guid.NewGuid();
    }
}
