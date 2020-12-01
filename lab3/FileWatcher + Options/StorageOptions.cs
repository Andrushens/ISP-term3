using System;

namespace lab3
{
    [Serializable]
    public class StorageOptions
    {
        public string SourceDirectory { get; set; }
        public string TargetDirectory { get; set; }

        public StorageOptions()
        {
            SourceDirectory = @"C:\Users\thela\source\repos\953506\term3\dot_net\lab3\SourceDirectory";
            TargetDirectory = @"C:\Users\thela\source\repos\953506\term3\dot_net\lab3\TargetDirectory";
        }
    }
}