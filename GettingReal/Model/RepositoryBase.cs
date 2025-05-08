namespace GettingReal.Model;

using System.IO;
using GettingReal.Handler;
/// <summary>
/// Base class for repositories.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class RepositoryBase<T> where T : new()
{
    private readonly XmlFileHandler<T> XmlFileHandler = new XmlFileHandler<T>();
    private string _filePath;
    /// <summary>
    /// List of items in the repository.
    /// </summary>
    public List<T> Items { get; private set; } = new List<T>();

    public RepositoryBase()
    {
        // Get the directory of the executing assembly
        var RealPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        if (RealPath != null)
        {
            RealPath = Path.Join(RealPath, "Data");
#if DEBUG
            _filePath = Path.Combine(RealPath, $"{typeof(T).Name}_repository_test.xml");
#else
            _filePath = Path.Combine(RealPath, $"{typeof(T).Name}_repository.xml");
#endif
        }

        // Check if the file exists, and if not, create it
        if (!File.Exists(_filePath))
        {
            using (var fs = File.Create(_filePath))
            {
                // File created
            }
        }

        // Load the repository from an XML file
        // Fixing the CS0308 error by removing the incorrect type argument
        Items = XmlFileHandler.Load(_filePath);

        // Check if the loaded items are null, and if so, initialize an empty list
        if (Items == null)
        {
            Items = new List<T>();
        }
    }

    /// <summary>
    /// Adds an item to the repository.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public void Add(T item)
    {
        // Generate a new GUID for the item
        Guid guid = Guid.NewGuid();

        // Check if the GUID already exists in the list
        while (Items.Any(x => (Guid)typeof(T).GetProperty("GUID")!.GetValue(x)! == guid))
            guid = Guid.NewGuid();

        // Set the GUID property of the item
        typeof(T).GetProperty("GUID")!.SetValue(item, guid);

        // Add the item to the list
        Items.Add(item);
        XmlFileHandler.Save(Items, _filePath);
    }

    /// <summary>
    /// Gets all items from the repository.
    /// </summary>
    /// <returns>A list of all items in the repository.</returns>
    public List<T> GetAll()
    {
        return Items;
    }

    /// <summary>
    /// Gets an item from the repository by its GUID.
    /// </summary>
    /// <param name="guid">The GUID of the item to get.</param>
    /// <returns>The item with the specified GUID, or null if not found.</returns>
    public T? Get(Guid guid)
    {
        if (typeof(T).GetProperty("GUID") != null)
        {
            return Items.FirstOrDefault(x => (Guid)typeof(T).GetProperty("GUID")!.GetValue(x)! == guid);
        }
        throw new InvalidOperationException("Type T does not contain a property named 'GUID'.");
    }

    /// <summary>
    /// Gets an item from the repository by its name.
    /// </summary>
    /// <param name="name">The name of the item to get.</param>
    /// <returns>The item with the specified name, or null if not found.</returns>
    public T? Get(string name)
    {
        return Items.FirstOrDefault(x => (string)typeof(T).GetProperty("Name")!.GetValue(x)! == name);
    }

    /// <summary>
    /// Updates an item in the repository.
    /// </summary>
    /// <param name="item">The item to update.</param>
    public void Update(T item)
    {
        var index = Items.ToList().IndexOf(item);
        if (index != -1)
        {
            Items.RemoveAt(index);
            Items.Insert(index, item);
        }
        XmlFileHandler.Save(Items, _filePath);
    }

    /// <summary>
    /// Removes an item from the repository.
    /// </summary>
    /// <param name="item"></param>
    public void Remove(T item)
    {
        Items.Remove(item);
        XmlFileHandler.Save(Items, _filePath);
    }

    public void Remove(Guid guid)
    {
        var item = Get(guid);
        if (item != null)
            Remove(item);
    }

    /// <summary>
    /// Clears all items from the repository.
    /// </summary>
    public void Clear()
    {
        Items.Clear();
        XmlFileHandler.Save(Items, _filePath);
    }
}
