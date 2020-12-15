using System;

namespace WatcherOptions
{
    [Serializable]
    public class CryptographyOptions
    {
        public byte[] Key { get; set; }
        public byte[] Iv { get; set; }

        public CryptographyOptions()
        {
            Key = System.Text.Encoding.UTF8.GetBytes("1q2w3e4r5t6y7u8i");
            Iv = System.Text.Encoding.UTF8.GetBytes("9O8I7U6Y5t4r3e2W");
        }
    }
}