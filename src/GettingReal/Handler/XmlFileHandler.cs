using System.IO;
using System.Xml.Serialization;

namespace GettingReal.Handler;

/// <summary>
/// Handles XML file operations for a generic type.
/// </summary>
/// <typeparam name="T"></typeparam>
public class XmlFileHandler<T> where T : new()
{
    /// <summary>
    /// Saves a repository to an XML file.
    /// </summary>
    /// <typeparam name="T">The type of the repository data.</typeparam>
    /// <param name="data">The data to save.</param>
    /// <param name="filePath">The file path to save the XML file.</param>
    public void Save(IEnumerable<T> data, string filePath)
    {
        try
        {
            var serializer = new XmlSerializer(typeof(List<T>));
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, data);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving to file: {ex.Message}");
        }
    }

    /// <summary>
    /// Loads a repository from an XML file.
    /// </summary>
    /// <typeparam name="T">The type of the repository data.</typeparam>
    /// <param name="filePath">The file path to load the XML file from.</param>
    /// <returns>The deserialized data.</returns>
    public List<T> Load(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return new List<T>();
            }

            var serializer = new XmlSerializer(typeof(List<T>));
            using (var reader = new StreamReader(filePath))
            {
                return (List<T>)serializer.Deserialize(reader);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading from file: {ex.Message}");
            return new List<T>();
        }
    }
}
