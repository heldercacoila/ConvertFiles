using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConvertFiles
{
    /// <summary>
    /// CSV file will inherit from MyFile to store their data in the Dictionary
    /// </summary>
    class CsvFile : MyFile
    {
        /// <summary>
        /// Receive the string with all the file regardless of their origin (Disk File / Database / etc)
        /// Extract the Header from the first line and values from the rest
        /// Then convert it to the Dictionary structure
        /// </summary>
        /// <param name="fileText"></param>
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

        /// <summary>
        /// Load data from Headear and Values into a Dictionary<string, object>
        /// Inner object are stored on new dictionaries 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="values"></param>
        private void LoadData(string[] header, List<string[]> values)
        {
            List<Dictionary<string, object>> myFile = new List<Dictionary<string, object>>();
            foreach (string[] row in values)
            {
                Dictionary<string, object> reg = new Dictionary<string, object>();
                for (int i = 0; i < header.Length; i++)
                {
                    string[] column = header[i].Split("_"); //Split Header by "_" to identify inner objects
                    FillData(reg, column, row[i]);
                }
                myFile.Add(reg);
            }
            Data = myFile;
        }

        /// <summary>
        /// This method creates the dictionary entries
        /// </summary>
        /// <param name="dict">The dictionary to fill</param>
        /// <param name="splitedHeader">Header with field names</param>
        /// <param name="value">Value of the field</param>
        /// <param name="position">Position where to start if the header has multiple positions (inner objects)</param>
        private void FillData(Dictionary<string, object> dict, string[] splitedHeader, object value, int position = 0)
        {
            if (splitedHeader.Length > 0)
            {
                //If header only have one position then is a simple property, just create the Key with header and value with the string on values
                if (splitedHeader.Length == position + 1)
                {
                    dict.Add(splitedHeader[position].Trim(), value.ToString().Trim());
                }
                else //If header have more then one position then is a complex object
                {
                    Dictionary<string, object> complexProperty;
                    object prop;
                    //Try to obtain the dictionary entry for the first header key
                    if (dict.TryGetValue(splitedHeader[position], out prop))
                    {
                        //If it exists then the value is another Dictionary. 
                        //Cast it and send it recursively to this method with position + 1 to fill the value for next header key
                        complexProperty = (Dictionary<string, object>)prop;
                        FillData(complexProperty, splitedHeader, value, position + 1);
                    }
                    else
                    {
                        //If it doesn't exist yet then create new Dictionary 
                        //Send it recursively to this method with position + 1 to fill the value for next header key 
                        //and then add it to the original dictionary
                        complexProperty = new Dictionary<string, object>();
                        FillData(complexProperty, splitedHeader, value, position + 1);
                        dict.Add(splitedHeader[position], complexProperty);
                    }
                }
            }
        }
    }
}
