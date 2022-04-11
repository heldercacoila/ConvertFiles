using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConvertFiles
{
    class CsvFile : MyFile
    {
        public CsvFile(string fileText)
        {
            List<string[]> values = new List<string[]>();
            string[] header = null;


            var strReader = new StringReader(fileText);

            while (strReader.Peek() > -1)
            {
                //1st Line Load Header
                if (header == null)
                {
                    header = strReader.ReadLine().Split(",");
                }
                else //Other lines load Values
                {
                    values.Add(strReader.ReadLine().Split(","));
                }
            }
            strReader.Close();

            LoadData(header, values);

        }

        private void LoadData(string[] header, List<string[]> values)
        {
            List<Dictionary<string, object>> myFile = new List<Dictionary<string, object>>();
            foreach (string[] row in values)
            {
                Dictionary<string, object> reg = new Dictionary<string, object>();
                for (int i = 0; i < header.Length; i++)
                {
                    string[] column = header[i].Split("_");
                    FillData(reg, column, row[i]);
                }
                myFile.Add(reg);
            }
            Data = myFile;
        }

        private void FillData(Dictionary<string, object> dict, string[] splitedHeader, object value, int position = 0)
        {
            if (splitedHeader.Length > 0)
            {
                if (splitedHeader.Length == position + 1)
                {
                    dict.Add(splitedHeader[position], value.ToString().Trim());
                }
                else
                {
                    Dictionary<string, object> complexProperty;
                    object prop;
                    if (dict.TryGetValue(splitedHeader[position], out prop))
                    {
                        complexProperty = (Dictionary<string, object>)prop;
                        FillData(complexProperty, splitedHeader, value, position + 1);
                    }
                    else
                    {
                        complexProperty = new Dictionary<string, object>();
                        FillData(complexProperty, splitedHeader, value, position + 1);
                        dict.Add(splitedHeader[position], complexProperty);
                    }
                }
            }
        }
    }
}
