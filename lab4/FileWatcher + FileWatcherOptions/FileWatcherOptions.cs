using System;

namespace WatcherOptions
{
    [Serializable]
    public class FileWatcherOptions
    {
        public StorageOptions StorageOptions { get; set; }
        public CryptographyOptions CryptographyOptions { get; set; }

        public FileWatcherOptions()
        {
            this.StorageOptions = new StorageOptions();
            this.CryptographyOptions = new CryptographyOptions();
        }
    }
}
