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
        /// <summary>
        /// Use XElement to create the XML structur based on the dictionary
        /// </summary>
        /// <param name="file">Object of class MyFile with the data inside a dictionary<string, object></param>
        /// <returns></returns>
        public string ConvertToXml(MyFile file)
        {
            //Create the root element and then use select to create new elements for each entrie on dictionary
            XElement el = new XElement("root",
                    file.Data.Select(row =>
                                new XElement("row", row.Select(x => ConvertDictionaryToXElement(x.Key, x.Value)))));


            return el.ToString();
        }

        /// <summary>
        /// Receive the Key and Value from the dictionary and analise it.
        /// If the value is another Dictionary then it call itself recursively to create the inner elements
        /// If the value is not a dictionary then it's the real value for the element
        /// </summary>
        /// <param name="key">Name for the element</param>
        /// <param name="value">Value for the element</param>
        /// <returns></returns>
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

        /// <summary>
        /// Just use the Newtonsoft.Json library to convert Dictionary to JSON
        /// </summary>
        /// <param name="file">Object of class MyFile with the data inside a dictionary<string, object></param>
        /// <returns></returns>
        public string ConvertToJson(MyFile file)
        {
            string result = JsonConvert.SerializeObject(file.Data, Formatting.Indented);

            return result;
        }
    }
}
