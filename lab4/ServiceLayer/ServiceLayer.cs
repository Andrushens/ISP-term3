using DAL;
using DMOptions;
using Generator;

namespace ServLayer
{
    public class ServiceLayer
    {
        private DataAccessLayer serviceLayer;
        private XmlGenerator xmlGenerator;

        public ServiceLayer(DataManagerOptions options)
        {
            serviceLayer = new DataAccessLayer(options);
        }

        public string CreateXmlFromDataBase()
        {
            var list = serviceLayer.GetEmployees();
            string xmlFileName;
            xmlGenerator = new XmlGenerator(list);
            xmlFileName = xmlGenerator.Generate();
            xmlGenerator.GenerateXsd();
            return xmlFileName;
        }
    }
}
