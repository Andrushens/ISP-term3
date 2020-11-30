using System;
using System.Xml.Serialization;

namespace lab3
{
    [Serializable]
    public class StorageOptions
    {
        public string SourceDirectory { get; set; }
        public string TargetDirectory { get; set; }
    }
}