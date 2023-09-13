using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS_lab_2
{
    public class Program
    {

        private static readonly string inputFilePath = "../../inputFile.txt";
        private static readonly string outputFilePath = "../../outputFile.txt";

        static void Main(string[] args)
        {
            string massage;
            int variant = 0;
            bool res = false;

            while (!res)
            {
                Console.WriteLine("Choose input string: console or file 1 or 2");
                res = int.TryParse(Console.ReadLine(), out variant);

                if (variant != 1 && variant != 2)
                {
                    res = false;
                }
            }

            if (variant == 1)
            {
                Console.WriteLine("Please enter input line");
                massage = Console.ReadLine();
            } 
            else if (variant == 2)
            {
                Console.WriteLine("Data has been picked from the file");                
                massage = ReadFile(inputFilePath);
            }
            else
            {
                Console.WriteLine("Error in program");
                massage = "";
            }

            string hashLine = MD5Algorythm.Algorythm(massage);
            Console.WriteLine(hashLine);

            variant = 0;
            res = false;

            while (!res)
            {
                Console.WriteLine("Would you like to store hash into the file? (1 - yes, 2 - no)");
                res = int.TryParse(Console.ReadLine(), out variant);

                if (variant != 1 && variant != 2)
                {
                    res = false;
                }
            }

            if (variant == 1)
            {
                // Записати дані у файл
                WriteFile(outputFilePath, hashLine);
            }
        }

        public static string ReadFile(string filePath)
        {
            try
            {
                string fileContents = File.ReadAllText(filePath, Encoding.ASCII);
                return fileContents;
            }
            catch (IOException e)
            {
                throw new IOException(e.Message);
            }
        }

        public static void WriteFile(string filePath, string data)
        {
            try
            {
                File.WriteAllText(filePath, data, Encoding.ASCII);
            }
            catch (IOException e)
            {
                throw new IOException(e.Message);
            }
        }
    }
}
