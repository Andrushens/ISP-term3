using System;
using System.IO;
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

                foreach(var prop in typeof(FileWatcherOptions).GetProperties())
                {
                    if(prop.Name == typeof(T).Name)
                    {
                        el = el.GetProperty("FileWatcherOptions");
                        foreach(var node in el.EnumerateObject())
                        {
                            if(node.Name == typeof(T).Name)
                            {
                                el = node.Value;
                                break;
                            }
                        }
                        break;
                    }
                }
                
                options = JsonSerializer.Deserialize<T>(el.GetRawText());
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
