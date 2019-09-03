using System;
using System.IO;

namespace FileSearcher
{

    class FileSearcher
    {
        static void Main(String[] args)
        {
            string regex = Console.ReadLine();
            Console.WriteLine("Received regex: " + regex);
            PrintFiles("C:\\Users\\roflm\\Desktop\\Carpeta");
        }
        
        static void FilesWithRegex(string regex, string directory)
        {
            
        }

        static void PrintFiles(string directory)
        {
            foreach(string f in Directory.GetFiles(directory))
            {
                Console.WriteLine(f);
            }

            foreach(string d in Directory.GetDirectories(directory))
            {
                PrintFiles(d);
            }
        }

        

    }

}