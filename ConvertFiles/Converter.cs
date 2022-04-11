using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ConvertFiles
{
    class Converter : IConvertToJson, IConvertToXml
    {
        public string ConvertToXml(MyFile file)
        {
            XElement el = new XElement("root",
                    file.Data.Select(row =>
                                new XElement("row", row.Select(x => ConvertDictionaryToXElement(x.Key, x.Value)))));


            return el.ToString();
        }

        private XElement ConvertDictionaryToXElement(string key, object value)
        {
            XElement result;
            if (value is Dictionary<string, object> dictionary)
            {
                result = new XElement(key, dictionary.Select(x => ConvertDictionaryToXElement(x.Key, x.Value)));
            }
            else
            {
                result = new XElement(key, value.ToString().Trim());
            }
            return result;
        }

        public string ConvertToJson(MyFile file)
        {
            string result = JsonConvert.SerializeObject(file.Data, Formatting.Indented);

            return result;
        }
    }
}
