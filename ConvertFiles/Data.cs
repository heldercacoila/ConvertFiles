using System.IO;

namespace ConvertFiles
{
    class Data
    {
        public string FromFile(string path)
        {
            string fileData = string.Empty;
            try
            {
                fileData = File.ReadAllText(path);
            }
            catch
            {
                //Some exception treatment could be applied here
            }

            return fileData;
        }

        public void WriteToFile(string path, string filename, string contents)
        {
            System.IO.Directory.CreateDirectory(path);
            File.WriteAllText(path + filename, contents);
        }


        public string FromDB()
        {
            //This scenario is outside of the scope of this exercise
            return string.Empty;
        }

        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }
    }
}
