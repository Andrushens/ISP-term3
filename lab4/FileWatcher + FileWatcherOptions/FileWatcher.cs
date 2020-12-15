using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using WatcherOptions;

namespace Watcher
{
    public class FileWatcher
    {
        private readonly DirectoryInfo sourceDirectory;
        private readonly DirectoryInfo targetDirectory;
        private readonly byte[] key;
        private readonly byte[] iv;
        private string lastFileExtention;

        public FileWatcher(FileWatcherOptions options)
        {
            this.sourceDirectory = new DirectoryInfo(options.StorageOptions.SourceDirectory);
            this.targetDirectory = new DirectoryInfo(options.StorageOptions.TargetDirectory);
            this.key = options.CryptographyOptions.Key;
            this.iv = options.CryptographyOptions.Iv;
        }

        public void StartProcess()
        {
            ClearDirectory(targetDirectory);
            Directory.CreateDirectory(targetDirectory.FullName);
            List<FileInfo> archivedFiles = new List<FileInfo>();
            bool isArchived;

            while (true)
            {
                foreach (FileInfo file in sourceDirectory.GetFiles())
                {
                    isArchived = false;

                    foreach (FileInfo archFile in archivedFiles)
                    {
                        if (file.Name == archFile.Name)
                        {
                            isArchived = true;
                            break;
                        }
                    }

                    if (!isArchived && file.Length != 0)
                    {
                        string zipsDirName = Path.Join(targetDirectory.FullName, "zips");
                        if (!Directory.Exists(zipsDirName))
                        {
                            Directory.CreateDirectory(zipsDirName);
                        }
                        Compress(file, new DirectoryInfo(zipsDirName));
                        Decompress(new FileInfo(Path.ChangeExtension(file.FullName, "gz")), new DirectoryInfo(zipsDirName));
                        archivedFiles.Add(file);
                    }
                }
                System.Threading.Thread.Sleep(500);
            }
        }

        private void Compress(FileInfo sourceFile, DirectoryInfo targetDir)
        {
            string arcFileName;
            string encryptedText;
            byte[] encryptedData;

            try
            {
                arcFileName = Path.Join(targetDir.FullName, sourceFile.Name);
                lastFileExtention = sourceFile.Extension;
                encryptedText = Encrypt(sourceFile);
                encryptedData = System.Text.Encoding.UTF8.GetBytes(encryptedText);

                using (FileStream targetStream = File.Create(arcFileName[0..^3] + "gz"))
                {
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        compressionStream.Write(encryptedData, 0, encryptedData.Length);
                    }
                }
                Console.WriteLine(sourceFile.Name + " comressed");
            }
            catch (Exception ex)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errors.txt");
                using StreamWriter sr = new StreamWriter(path, true);
                sr.Write(ex.Message);
            }
        }

        private void Decompress(FileInfo sourceFile, DirectoryInfo targetDir)
        {
            string arcFileName = Path.Join(targetDir.FullName, sourceFile.Name);
            FileInfo decompressedFile;
            string decryptedText;

            try
            {
                decompressedFile = new FileInfo(Path.ChangeExtension(arcFileName, lastFileExtention));

                using (FileStream sourceStream = new FileStream(arcFileName, FileMode.Open))
                {
                    using (FileStream targetStream = File.Create(decompressedFile.FullName))
                    {
                        using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                        {
                            decompressionStream.CopyTo(targetStream);
                        }
                    }
                }
                decryptedText = Decrypt(decompressedFile);

                using (StreamWriter writeS = new StreamWriter(decompressedFile.FullName))
                {
                    writeS.Write(decryptedText);
                }
                PutInDirectory(decompressedFile, new DirectoryInfo(Path.Join(targetDir.Parent.FullName, "txts")));
                File.Delete(decompressedFile.FullName);
                Console.WriteLine(sourceFile.Name + " decompressed");
            }
            catch (Exception ex)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errors.txt");
                using StreamWriter sr = new StreamWriter(path, true);
                sr.Write(ex.Message);
            }
        }

        private string Encrypt(FileInfo file)
        {
            byte[] sourceData = null;
            byte[] encryptedData = null;

            Aes Aes = Aes.Create();

            using (FileStream fs = File.OpenRead(file.FullName))
            {
                sourceData = new byte[fs.Length];
                fs.Read(sourceData, 0, sourceData.Length);
            }

            using (MemoryStream memStream = new MemoryStream())
            {
                using (CryptoStream encrypStream = new CryptoStream(memStream, Aes.CreateEncryptor(key, iv), CryptoStreamMode.Write))
                {
                    encrypStream.Write(sourceData, 0, sourceData.Length);
                    encrypStream.FlushFinalBlock();
                    encryptedData = memStream.ToArray();
                }
            }

            return Convert.ToBase64String(encryptedData);
        }

        private string Decrypt(FileInfo archivedFile)
        {
            byte[] encryptedData = null;
            byte[] decryptedData = null;
            byte[] buffer = new byte[1024];

            using (StreamReader readS = new StreamReader(archivedFile.FullName))
            {
                encryptedData = Convert.FromBase64String(readS.ReadToEnd());
            }

            Aes Aes = Aes.Create();

            using (MemoryStream memStream = new MemoryStream(encryptedData))
            {
                using (CryptoStream decrypStream = new CryptoStream(memStream, Aes.CreateDecryptor(key, iv), CryptoStreamMode.Read))
                {
                    using (MemoryStream tempMem = new MemoryStream())
                    {

                        while ((decrypStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            tempMem.Write(buffer, 0, buffer.Length);
                        }
                        decryptedData = tempMem.ToArray();
                    }
                }
            }

            return System.Text.Encoding.UTF8.GetString(decryptedData);
        }

        private void PutInDirectory(FileInfo file, DirectoryInfo targetDir)
        {
            string dirName = targetDir.FullName;
            string targetFileName = Path.Combine(targetDir.FullName, file.Name);

            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }

            if (!File.Exists(targetFileName))
            {
                File.Copy(file.FullName, targetFileName);
            }
        }

        private void ClearDirectory(DirectoryInfo directory)
        {
            try
            {
                foreach (DirectoryInfo dir in directory.GetDirectories())
                {
                    ClearDirectory(dir);
                    dir.Delete();
                }

                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }
            }
            catch (Exception ex)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errors.txt");
                using StreamWriter sr = new StreamWriter(path, true);
                sr.Write(ex.Message);
            }
        }
    }
}