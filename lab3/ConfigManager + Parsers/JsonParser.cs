using System;
using System.IO;
using System.Text.Json;

namespace lab3
{
    public class JsonParser : IConfigParser
    {
        public T Parse<T>(string jsonConfigFileName) where T : new()
        {
            T options = new T();

            try
            {
                string jsonInner = File.ReadAllText(jsonConfigFileName);
                JsonDocument jDoc = JsonDocument.Parse(jsonInner);
                JsonElement root = jDoc.RootElement.GetProperty(typeof(FileWatcherOptions).Name);
                JsonElement prop = root.GetProperty(typeof(T).Name);
                options = JsonSerializer.Deserialize<T>(prop.GetRawText());
            }
            catch (Exception ex)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errors.txt");
                using StreamWriter sr = new StreamWriter(path, true);
                sr.Write(ex.Message);
            }

            return options;
        }
    }
}