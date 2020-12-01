using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace lab3
{
    public class JsonParser : IConfigParser
    {
        public T Parse<T> (string jsonConfigFileName) where T : new()
        {
            T options = new T();

            try
            {
                string jsonInner = File.ReadAllText(jsonConfigFileName);
                JsonDocument jDoc = JsonDocument.Parse(jsonInner);
                var el = jDoc.RootElement;

                if (typeof(FileWatcherOptions).GetProperties().Any(prop => prop.Name == typeof(T).Name))
                {
                    el = el.GetProperty("FileWatcherOptions");
                }
                el = el.EnumerateObject().Single(pr => pr.Name == typeof(T).Name).Value;
                options = JsonSerializer.Deserialize<T>(el.GetRawText());
            }
            catch (Exception ex)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory[0..^25], "errors.txt");
                using StreamWriter sr = new StreamWriter(path, true);
                sr.Write(ex.Message);
            }

            return options;
        }
    }
}
