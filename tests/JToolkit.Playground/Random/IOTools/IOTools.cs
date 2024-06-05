using Newtonsoft.Json;

namespace JToolkit.Playground.Random.IOTools;

public static class IOTools
{
    // Write data to a JSON file
    public static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
    {
        TextWriter writer = null;
        try
        {
            var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);
            writer = new StreamWriter(filePath, append);
            writer.Write(contentsToWriteToFile);
        }
        finally
        {
            if (writer != null)
                writer.Close();
        }
    }

    public static T ReadFromJsonFile<T>(string filePath) where T : new()
    {
        TextReader reader = null;
        try
        {
            reader = new StreamReader(filePath);
            string contents = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(contents);
        }
        finally
        {
            if (reader != null)
                reader.Close();
        }
    }
}