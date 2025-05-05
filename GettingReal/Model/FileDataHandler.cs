using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace GettingReal.Model;

internal class FileDataHandler
{
    private string FilePath;

    public FileDataHandler(string filePath)
    {
        this.FilePath = filePath;
    }


    // __________METODER__________

    // burde virke
    public void AppendLine(string input)
    {
        using (StreamWriter sw = new StreamWriter(FilePath, true))
        {
            sw.WriteLine(input);
        }
    }

    public void WriteFile()
    {
        var lines;
        // write list to file
        using (StreamWriter sw = new StreamWriter(FilePath, false))
        {
            // write list to file
            foreach (var line in lines)
            {
                sw.WriteLine(line);
            }
        }
    }


    public string ReadLine<T>(T item) where T : ISaveable<T>, new()
    {
        List<T> list = ReadAllLines<T>();
        // find line
        return "";
    }

    // burde virke
    public List<T> ReadAllLines<T>() where T : ISaveable<T>, new()
    {
        List<T> result = new List<T>();
        using (StreamReader sr = new StreamReader(FilePath))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    T instance = new T();
                    result.Add(instance.FromString(line));
                }
            }
        }     
        return result;
    }

    // Burde virke
    public void DeleteLine<T>(T item) where T : ISaveable<T>, new()
    {
        List<T> list = ReadAllLines<T>();
        list.RemoveAll(line => line.Equals(item));
        WriteFile();
    }

}
