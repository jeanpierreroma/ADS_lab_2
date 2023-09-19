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

        // Результат роботи програми залежить від кодингу Encoding
        // Тобто для консолі треба брати:
        // Латиниця: ASCII, Default, UTF7, UTF8
        // Українська: нічого (перевіряв відповідно до сайту)
        // Для файлу:
        // Латинська: BigEndianUnicode, Unicode
        // Українська: нічого (перевіряв відповідно до сайту)

        private static readonly string inputFilePath = "../../inputFile.txt";
        private static readonly string outputFilePath = "../../outputFile.txt";

        static void Main(string[] args)
        {
            StringBuilder massage = new StringBuilder();
            string hashLine;

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
                massage = new StringBuilder(Console.ReadLine());
            }
            else if (variant == 2)
            {
                Console.WriteLine("Data has been picked from the file");
                massage = ReadFile(inputFilePath);
            }
            else
            {
                Console.WriteLine("Error in program");
                massage = new StringBuilder("");
            }

            hashLine = MD5Algorythm.MakeHashLine(massage);
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

        public static StringBuilder ReadFile(string filePath)
        {
            StringBuilder content = new StringBuilder();
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (StreamReader reader = new StreamReader(fileStream, Encoding.ASCII))
                {
                    //string line;
                    //while ((line = reader.ReadLine()) != null)
                    //{
                    //    content.Append(line);
                    //}

                    int character;

                    while ((character = reader.Read()) != -1)
                    {
                        char ch = (char)character;
                        if (ch == '\r')
                        {
                            continue;
                        }

                        content.Append(ch);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during reading file: " + ex.Message);
            }

            return content;
        }

        public static void WriteFile(string filePath, string data)
        {
            try
            {
                File.WriteAllText(filePath, data, Encoding.Default);
            }
            catch (IOException e)
            {
                throw new IOException(e.Message);
            }
        }
    }
}