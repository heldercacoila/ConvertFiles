using System;

namespace ConvertFiles
{
    class Menu
    {

        public string OriginalFile { get; set; }
        public string DestinationFilename { get; set; }
        public EnumFileType OriginalType { get; set; }
        public EnumFileType DestinationType { get; set; }
        public bool Quit { get; set; }
        public bool CheckFileExist { get; set; }
        public string DestinationPath { get; set; }

        public string CompleteDestinationFilename
        {
            get
            {
                return DestinationFilename + "." + DestinationType.ToString();
            }
        }

        public bool IsComplete
        {
            get
            {
                return !string.IsNullOrEmpty(OriginalFile) && !string.IsNullOrEmpty(DestinationFilename) && DestinationType != EnumFileType.NONE;
            }
        }

        /// <summary>
        /// The object can be created with some values already defined
        /// </summary>
        public Menu(string originalFile, EnumFileType originalType, string destinationPath, string destinationFilename, EnumFileType destinationType, bool checkFileExist)
        {
            OriginalFile = originalFile;
            OriginalType = originalType;
            DestinationPath = destinationPath;
            DestinationFilename = destinationFilename;
            DestinationType = destinationType;
            CheckFileExist = checkFileExist;
        }

        /// <summary>
        /// Show a menu to get all the necessary values to convertion (Origin / Destination / Type)
        /// </summary>
        public void MainMenu()
        {
            if (string.IsNullOrEmpty(OriginalFile))
            {
                //Only show menu to get original file if it isn't already filled
                OriginalFile = PathMenu(false);
                if (string.IsNullOrEmpty(OriginalFile))
                {
                    Quit = true;
                    return;
                }
            }

            if (DestinationType == EnumFileType.NONE)
            {
                DestinationType = MenuConvertTo();
                if (DestinationType == EnumFileType.NONE)
                {
                    //Get back to original file selection
                    OriginalFile = string.Empty;
                    return;
                }
            }

            if (DestinationType != EnumFileType.NONE)
            {
                if (string.IsNullOrEmpty(DestinationFilename))
                {
                    DestinationFilename = PathMenu(true);
                    if (string.IsNullOrEmpty(DestinationFilename))
                    {
                        //If filename is empty then go back to previous menu
                        DestinationType = EnumFileType.NONE;
                        return;
                    }
                }
            }
            else
            {
                Console.Clear();
            }
        }

        /// <summary>
        /// This Method shows a menu to get the file path of the file to convert
        /// </summary>
        /// <returns>Path of file to convert</returns>
        private string PathMenu(bool destination)
        {
            string filename = string.Empty;
            bool exit = false;
            while (!exit)
            {
                if (destination)
                {
                    Console.WriteLine("Enter the destination filename or 'b' to go back");
                }
                else
                {
                    Console.WriteLine("Enter path to " + OriginalType.ToString() + " file or 'q' to quit");
                }

                string str = Console.ReadLine();

                if (destination)
                {
                    if (str.ToLower() != "b")
                    {
                        if (!CheckFileExist || !Data.FileExists(DestinationPath + str + "." + DestinationType.ToString()))
                        {
                            filename = str;
                            exit = true;
                        }
                        else
                        {
                            Console.WriteLine("File exist, do you want to override? (Y)es, (N)o.");
                            string yesOrNo = Console.ReadLine();
                            while (yesOrNo.ToLower() != "y" && yesOrNo.ToLower() != "n")
                            {
                                Console.WriteLine("Invalid Option");
                                Console.WriteLine("File exist, do you want to override? (Y)es, (N)o.");
                                yesOrNo = Console.ReadLine();
                            }

                            if (yesOrNo.ToLower() == "y")
                            {
                                filename = str;
                                exit = true;
                            }
                        }
                    }
                    else
                    {
                        exit = true;
                    }
                }
                else
                {
                    if (str.ToLower() != "q")
                    {
                        if (!Data.FileExists(str))
                        {
                            Console.WriteLine("Invalid Path.");
                        }
                        else
                        {
                            filename = str;
                            exit = true;
                        }
                    }
                    else
                    {
                        exit = true;
                    }
                }
            }
            return filename;
        }

        /// <summary>
        /// Show a menu to get the destination Type file 
        /// </summary>
        /// <returns></returns>
        private EnumFileType MenuConvertTo()
        {
            Console.WriteLine("Transform " + OriginalType.ToString() + " into:");
            Console.WriteLine("1 - Json");
            Console.WriteLine("2 - XML");
            Console.WriteLine("3 - Back");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("Json");
                    return EnumFileType.JSON;
                case "2":
                    Console.WriteLine("XML");
                    return EnumFileType.XML;
                case "3":
                    return EnumFileType.NONE;
                default:
                    Console.WriteLine("Invalid Option");
                    return MenuConvertTo();

            }
        }
    }
}
