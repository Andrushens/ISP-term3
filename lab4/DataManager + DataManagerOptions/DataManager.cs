using System.IO;
using DMOptions;
using ServLayer;
using Transfer;

namespace DM
{
    public class DataManager
    {
        private readonly string targetDirectory;
        private readonly ServiceLayer serviceLayer;
        private readonly FileTransfer fileTransfer;

        public DataManager(DataManagerOptions options = null)
        {
            options ??= new DataManagerOptions();
            serviceLayer = new ServiceLayer(options);
            this.targetDirectory = options.StorageOptions.TargetDirectory;
            fileTransfer = new FileTransfer();
        }

        public void Process()
        {
            string xmlFileName;
            xmlFileName = serviceLayer.CreateXmlFromDataBase();
            fileTransfer.Transfer(xmlFileName, targetDirectory);
            FileInfo xmlPathFile = new FileInfo(xmlFileName);
            string xsdFileName = Path.ChangeExtension(Path.Combine(xmlPathFile.DirectoryName, "XSD_") + xmlPathFile.Name, "xsd");
            fileTransfer.Transfer(xsdFileName, targetDirectory);
        }
    }
}
