using System;
using System.IO;

namespace Transfer
{
    public class FileTransfer
    {
        public void Transfer(string sourceFileName, string destFolderName)
        {
            FileInfo sourceFile = new FileInfo(sourceFileName);
            DirectoryInfo destinationFolder = new DirectoryInfo(destFolderName);

            try
            {
                string destFileName = Path.Combine(destinationFolder.FullName, sourceFile.Name);
                if (File.Exists(destFileName))
                {
                    File.Delete(destFileName);
                }
                File.Copy(sourceFile.FullName, destFileName);
                File.Delete(sourceFile.FullName);
            }
            catch(Exception ex)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errors.txt");
                using StreamWriter sr = new StreamWriter(path, true);
                sr.Write(ex.Message);
            }
        }
    }
}
