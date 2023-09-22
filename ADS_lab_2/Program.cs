using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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

        // UTF7 - працює для моїх файлів

        // для inputFile - правильно
        // для inputFile_ua - правильно

        private static readonly string inputFilePath = "../../test2_2.txt";
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

                //byte[] arrayMessage = Encoding.UTF8.GetBytes(massage.ToString());
                byte[] arrayMessage = Encoding.UTF8.GetBytes("Привіт");


                buffer = MD5Algorythm.Algorithm(arrayMessage);
            }
            else if (variant == 2)
            {
                Console.WriteLine("Data has been picked from the file");
                //Stopwatch stopwatch = new Stopwatch();

                //stopwatch.Start();

                buffer = ReadFile(inputFilePath);

                //stopwatch.Stop();

                //long elapsedTimeMilliseconds = stopwatch.ElapsedMilliseconds;

                //Console.WriteLine("Час виконання функції: " + elapsedTimeMilliseconds + " мілісекунд");


                Console.WriteLine("Success!");
            }
            else
            {
                Console.WriteLine("Error in program");
                massage = new StringBuilder("");

                byte[] arrayMessage = Encoding.Default.GetBytes(massage.ToString());

                buffer = MD5Algorythm.Algorithm(arrayMessage);
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
                uint counter = 0;
                StringBuilder sb = new StringBuilder();

                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (StreamReader reader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    int character;
                    while ((character = reader.Read()) != -1)
                    {
                        counter++;
                        sb.Append((char)character);
                    }

                }

                byte[] buffer = Encoding.UTF8.GetBytes(sb.ToString());
                tmp_res = MD5Algorythm.Algorithm(buffer);



                Console.WriteLine(counter);

                //using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                //using (StreamReader reader = new StreamReader(fileStream, Encoding.UTF7))
                //{
                //    int character;

                //    byte[] buffer = new byte[counter];

                //    int i = 0;

                //    while((character = reader.Read()) != -1)
                //    {
                //        //byte[] newBufer = new byte[buffer.Length + 1];

                //        //Array.Copy(buffer, newBufer, buffer.Length);

                //        char ch = (char)character;

                //        byte byteUTF8Array = (byte)ch;

                //        buffer[i] = byteUTF8Array;

                //        i++;

                //        //Array.Copy(new byte[] { byteUTF8Array }, 0, newBufer, buffer.Length, 1);

                //        //buffer = newBufer;
                //    }

                //    tmp_res = MD5Algorythm.Algorithm(buffer);
                //}

                //using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                //using (StreamReader reader = new StreamReader(fileStream, Encoding.UTF8))
                //{
                //    int character;
                //    int i = 0;


                //    byte[] arrayMassage = new byte[counter];
                //    while ((character = reader.Read()) != -1)
                //    {
                //        char deleteVar = (char)character;

                //        //arrayMassage[i] = (byte)character;
                //        //i++;
                //    }

                //    tmp_res = MD5Algorythm.Algorithm(arrayMassage);
                //}
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



        public static byte[] Remove45After43(byte[] inputArray)
        {
            if (inputArray == null || inputArray.Length == 0)
                return inputArray;

            var result = new byte[inputArray.Length];
            int currentIndex = 0;
            bool previousWas43 = false;

            for (int i = 0; i < inputArray.Length; i++)
            {
                if (i == 0 || inputArray[i] != 45 || !previousWas43)
                {
                    result[currentIndex] = inputArray[i];
                    currentIndex++;
                    previousWas43 = (inputArray[i] == 43);
                }
            }

            Array.Resize(ref result, currentIndex);
            return result;
        }
    }
}