using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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


        // Є ідея просто написати новий алгоритм для файлу

        private static readonly string inputFilePath = "../../Luannacaputto.jpg";
        private static readonly string outputFilePath = "../../outputFile.txt";

        static void Main(string[] args)
        {
            StringBuilder massage = new StringBuilder();
            uint[] buffer;

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

                buffer = MD5Algorythm.AlgorithmConsole(massage.ToString());
            }
            else if (variant == 2)
            {
                Console.WriteLine("Data has been picked from the file");
                // Створення екземпляру Stopwatch
                Stopwatch stopwatch = new Stopwatch();

                // Початок вимірювання часу
                stopwatch.Start();

                // Виклик функції, яку ви хочете виміряти
                buffer = ReadFile(inputFilePath);

                // Зупинка вимірювання часу
                stopwatch.Stop();

                // Отримання часу в мілісекундах
                long elapsedTimeMilliseconds = stopwatch.ElapsedMilliseconds;

                Console.WriteLine("Час виконання функції: " + elapsedTimeMilliseconds + " мілісекунд");


                Console.WriteLine("Success!");
            }
            else
            {
                Console.WriteLine("Error in program");
                massage = new StringBuilder("");

                buffer = MD5Algorythm.AlgorithmConsole(massage.ToString());
            }

            string hashLine = MD5Algorythm.MakeStringFromArrUInt(buffer);
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

        public static uint[] ReadFile(string filePath)
        {
            uint[] tmp_res = { 0x67452301, 0xEFCDAB89, 0x98BADCFE, 0x10325476 };
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (StreamReader reader = new StreamReader(fileStream, Encoding.Default))
                {
                    int character;
                    uint counter = 0;

                    StringBuilder content = new StringBuilder();
                    while ((character = reader.Read()) != -1)
                    {
                        char ch = (char)character;
                        if (ch == '\r')
                        {
                            continue;
                            counter--;
                        }

                        content.Append(ch);
                        counter++;

                        if (counter == 1_204_000)
                        {
                            tmp_res = MD5Algorythm.AlgorithmFile(content.ToString(), tmp_res[0], tmp_res[1], tmp_res[2], tmp_res[3]);

                            content.Clear();

                            counter = 0;
                        }
                    }
                    tmp_res = MD5Algorythm.AlgorithmFile(content.ToString(), tmp_res[0], tmp_res[1], tmp_res[2], tmp_res[3]);

                    content.Clear();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during reading file: " + ex.Message);
            }

            return tmp_res;
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