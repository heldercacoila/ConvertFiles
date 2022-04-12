using System;
using System.Collections.Generic;
using System.Text;

namespace ConvertFiles
{
    class MyFile
    {
        /// <summary>
        /// All type of files (CSV, JSON, XML) will always store their data in this dictionary
        /// </summary>
        public List<Dictionary<string, object>> Data { get; set; }
    }
}
