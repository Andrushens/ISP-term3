using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ConfigProvider
{
    public class XmlParser : IConfigParser
    {
        public T Parse<T> (string xmlConfigFileName) where T : new()
        {
            T options = new T();

            try
            {
                Validate(xmlConfigFileName);
                string xmlInner = "";
                XmlSerializer formatter = new XmlSerializer(typeof(T));
                XDocument xDoc = XDocument.Load(xmlConfigFileName);
                var nodes = xDoc.Document.Descendants();

                foreach (var node in nodes)
                {
                    if (node.Name == typeof(T).Name)
                    {
                        xmlInner = node.ToString();
                        break;
                    }
                }
                using (TextReader tReader = new StringReader(xmlInner))
                {
                    options = (T)formatter.Deserialize(tReader);
                }
            }
            catch (Exception ex)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errors.txt");
                using StreamWriter sr = new StreamWriter(path, true);
                sr.Write(ex.Message);
            }

            return options;
        }

        private void Validate(string xmlConfigFileName)
        {
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add(null, Path.ChangeExtension(xmlConfigFileName, "xsd"));
            XDocument xDoc = XDocument.Load(xmlConfigFileName);

            xDoc.Validate(schemas, (sender, validationEventArgs) =>
            {
                if (validationEventArgs.Message.Length != 0)
                {
                    throw validationEventArgs.Exception;
                }
            });
        }
    }
}