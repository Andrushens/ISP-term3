using System;
using System.IO;
using System.Text.Json;
using WatcherOptions;

namespace ConfigProvider
{
    public class JsonParser : IConfigParser
    {
        public T Parse<T>(string jsonConfigFileName) where T : new()
        {
            T options = new T();

            try
            {
                string jsonInner = File.ReadAllText(jsonConfigFileName);
                options = JsonSerializer.Deserialize<T>(jsonConfigFileName);
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