using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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

        // Нарешті пройшло на 600 мБ
        // Порівняти файли buffer на розмір та на відповідність

        private static readonly string inputFilePath = "../../test2.dat";
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

                byte[] arrayMessage = Encoding.UTF8.GetBytes(massage.ToString());

                buffer = MD5Algorythm.Algorithm(arrayMessage);
            }
            else if (variant == 2)
            {
                Console.WriteLine("Data has been picked from the file");

                byte[] fileContent;
                var size = ReadFile(inputFilePath, out fileContent);

                buffer = MD5Algorythm.Algorithm(fileContent, true, size);

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

        public static ulong ReadFile(string filePath, out byte[] content)
        {
            content = null;
            ulong counter = 0;

            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (StreamReader reader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    int character;
                    while ((character = reader.Read()) != -1)
                    {
                        counter += (ulong)Encoding.UTF8.GetBytes(new char[] { (char)character }).Length;
                    }

                }

                // Заради продуктивності зробимо одну хуйню
                ulong paddingLength = 64 - ((counter + 8) % 64);  // 64 біти (8 байтів) для зберігання довжини повідомлення
                if (paddingLength < 1)
                    paddingLength += 64;

                ulong newLength = counter + paddingLength + 8;  // Додаємо 8 байтів для зберігання довжини повідомлення

                Console.WriteLine(newLength);

                content = new byte[newLength];

                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (StreamReader reader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    ulong destinationIndex = 0;

                    int character;
                    while ((character = reader.Read()) != -1)
                    {
                        var bytes = Encoding.UTF8.GetBytes(new char[] { (char)character });

                        ArrayCopy(bytes, 0, content, destinationIndex, (ulong)bytes.Length);
                        destinationIndex += (ulong)bytes.Length;
                    }
                }

                ////////////////////////////////////////////////////////

                //uint counter = 0;
                //StringBuilder sb = new StringBuilder();

                //using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                //using (StreamReader reader = new StreamReader(fileStream, Encoding.UTF8))
                //{
                //    int character;
                //    while ((character = reader.Read()) != -1)
                //    {
                //        counter++;
                //        sb.Append((char)character);
                //    }

                //}

                //buffer = Encoding.UTF8.GetBytes(sb.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during reading file: " + ex.Message);
            }

            return counter;
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

        public static void ArrayCopy(byte[] arraySource, ulong sourceIndex, byte[] arrayDestination, ulong destinationIndex, ulong length)
        {
            for (ulong i = sourceIndex; i < length; i++)
            {
                arrayDestination[destinationIndex] = arraySource[i];
                destinationIndex++;
            }
        }
    }
}