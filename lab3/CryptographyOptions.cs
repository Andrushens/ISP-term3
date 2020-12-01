using System;

namespace lab3
{
    [Serializable]
    public class CryptographyOptions
    {
        public byte[] Key { get; set; }
        public byte[] Iv { get; set; }
    }
}