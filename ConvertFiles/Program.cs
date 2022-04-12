using System;
using System.Diagnostics;

namespace ConvertFiles
{
    class Program
    {
        readonly static string outputPath = AppDomain.CurrentDomain.BaseDirectory + "OUTPUT\\";
        static void Main(string[] args)
        {
            string originalFile, destinationFilename;
            originalFile = destinationFilename = string.Empty;
            EnumFileType destinationType = EnumFileType.NONE;

            //Check if some parameters were passed
            if (args.Length > 0)
            {
                //1st parameter = originalFile
                originalFile = args[0];

                if (args.Length > 1)
                {
                    //2nd parameter = destinationType
                    Enum.TryParse(args[1], out destinationType);
                }

                if (args.Length > 2)
                {
                    //3rd parameter = destinationFilename
                    destinationFilename = args[2];
                }
            }

            //Create Menu for CSV original File Type, and check if destination file exists 
            Menu myMenu = new Menu(originalFile, EnumFileType.CSV, outputPath, destinationFilename, destinationType, true);
            if (myMenu.IsComplete)
            {
                if (ProcessConversion(myMenu.OriginalFile, myMenu.CompleteDestinationFilename, myMenu.DestinationType))
                {
                    OpenDestination(myMenu.CompleteDestinationFilename);
                    Console.WriteLine("Conversion completed");
                }
                myMenu.OriginalFile = myMenu.DestinationFilename = string.Empty;
                myMenu.DestinationType = EnumFileType.NONE;
            }

            while (!myMenu.Quit)
            {
                myMenu.MainMenu();
                if (myMenu.IsComplete)
                {
                    Console.Clear();
                    if (ProcessConversion(myMenu.OriginalFile, myMenu.CompleteDestinationFilename, myMenu.DestinationType))
                    {
                        OpenDestination(myMenu.CompleteDestinationFilename);
                        Console.WriteLine("Conversion  completed");
                    }
                }
                //Back to start
                myMenu.OriginalFile = string.Empty;
                myMenu.DestinationFilename = string.Empty;
                myMenu.DestinationType = EnumFileType.NONE;
            }

        }

        private static bool ProcessConversion(string originalFile, string destinationFilename, EnumFileType destinationType)
        {
            bool result = false;
            string output = string.Empty;

            Console.WriteLine("Converting \"" + originalFile + "\" to " + destinationFilename);

            //Create CSVFile Object with original file from disk
            Data data = new Data();
            CsvFile file = null;
            try
            {
                file = new CsvFile(data.FromFile(originalFile));
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error reading file.");
                Console.WriteLine("Error message: " + ex.Message);
            }
            
            if(file != null)
            {
                Converter converter = new Converter();
                //Convert file to destination type
                switch (destinationType)
                {
                    case EnumFileType.JSON:
                        output = converter.ConvertToJson(file);
                        break;
                    case EnumFileType.XML:
                        output = converter.ConvertToXml(file);
                        break;
                    default:
                        Console.WriteLine("Ivalid Format");
                        break;
                }

                if (!string.IsNullOrEmpty(output))
                {
                    try
                    {
                        //Write Destination file to disk
                        data.WriteToFile(outputPath, destinationFilename, output);
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error writing file.");
                        Console.WriteLine("Error message: " + ex.Message);
                        return result;
                    }
                }
            }
            return result;
        }

        private static void OpenDestination(string filename)
        {
            Process.Start("explorer", " /select, \"" + outputPath + filename + "\"");
        }
    }
}
