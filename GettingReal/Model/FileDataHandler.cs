using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        using (StreamWriter sw = new StreamWriter(FilePath))
        {
            sw.WriteLine(input);
        }
    }

    public string ReadLine(int id)
    {
        // logik til at læse en linje fra fil
        // find line
        // returner
        return "";
    }

    // burde virke
    public IEnumerable<T> ReadAllLines<T>(ISaveable<T> type) where T : ISaveable<T>, new()
    {
        List<T> result = new List<T>();
        using (StreamReader sr = new StreamReader(FilePath))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    result.Add(new T().FromString(line));
                }
            }

        }     
        return new List<T>();
    }

    public void DeleteLine(string id)
    {
        //logik til at fjerne en linje fra en fil
        // find line
        // fjern line
    }
}
