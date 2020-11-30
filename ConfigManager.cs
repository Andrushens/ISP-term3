namespace lab3
{
    public class ConfigManager
    {
        private readonly IConfigParser configParser;

        public ConfigManager(IConfigParser configParser)
        {
            this.configParser = configParser;
        }

        public T GetOptions<T>(string fileOptionsPath) where T : new()
        {
            return configParser.Parse<T>(fileOptionsPath);
        }
    }
}