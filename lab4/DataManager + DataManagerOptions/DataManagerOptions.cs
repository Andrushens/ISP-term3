using System;
using WatcherOptions;

namespace DMOptions
{
    [Serializable]
    public class DataManagerOptions
    {
        public DataBaseOptions DataBaseOptions { get; set; }
        public StorageOptions StorageOptions { get; set; }

        public DataManagerOptions()
        {
            DataBaseOptions = new DataBaseOptions();
            StorageOptions = new StorageOptions();
        }
    }
}
