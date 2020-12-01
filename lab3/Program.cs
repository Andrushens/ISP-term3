using System;
using System.IO;

namespace lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            string workDirName = AppDomain.CurrentDomain.BaseDirectory;
            string xmlConfigFileName = Path.Join(workDirName, "config.xml");
            string jsonConfigFileName = Path.Join(workDirName, "appsettings.json");
            FileWatcherOptions options = new FileWatcherOptions();
            
            ConfigManager configManager = new ConfigManager(new XmlParser());
            options.CryptographyOptions = configManager.GetOptions<CryptographyOptions>(xmlConfigFileName);

            configManager = new ConfigManager(new JsonParser());
            options.StorageOptions = configManager.GetOptions<StorageOptions>(jsonConfigFileName);

            FileWatcher watcher = new FileWatcher(options);
            watcher.StartProcess();
        }
    }
}