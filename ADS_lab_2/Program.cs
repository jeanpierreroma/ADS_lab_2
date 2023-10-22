using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace ADS_lab_2
{
    public class Program
    {
        private static readonly string inputFilePath = "../../test2GB.dat";
        private static readonly string outputFilePath = "../../outputFile.txt";

        static void Main(string[] args)
        {
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
                string massage = Console.ReadLine();

                byte[] arrayMessage = Encoding.UTF8.GetBytes(massage.ToString());

                buffer = MD5Algorythm.Algorithm(arrayMessage);
            }
            else if (variant == 2)
            {
                Console.WriteLine("Data has been picked from the file");

                buffer = WorkWithFile(inputFilePath);

                Console.WriteLine("Success!");
            }
            else
            {
                Console.WriteLine("Error in program");

                byte[] arrayMessage = Encoding.Default.GetBytes("");

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
        public static uint[] WorkWithFile(string filePath)
        {
            uint[] MD5Result = null;

            // Спочатку визначаємо кількість значень у файлі
            ulong size = CountSizeOfFile(filePath);
            Console.WriteLine($"File size: {size}");

            // Якщо розмір менше 100 МБ просто зчитуємо цей файл
            uint size100MB = 128 * 1024 * 1024;

            if (size < size100MB)
            {
                StringBuilder sb = new StringBuilder();

                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (StreamReader reader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    int character;
                    while ((character = reader.Read()) != -1)
                    {
                        sb.Append((char)character);
                    }

                }

                var buffer = Encoding.UTF8.GetBytes(sb.ToString());

                MD5Result = MD5Algorythm.Algorithm(buffer);
            }
            // Якщо розмір більше 100 МБ розбиваємо його порціями по 100 МБ
            else
            {
                //Наперед визначаємо доповнювальний розмір нашого повідомлення для хешування
                ulong paddingLength = 64 - ((size + 8) % 64);
                if (paddingLength < 1)
                    paddingLength += 64;

                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (StreamReader reader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    int character = 0;
                    while (character != -1)
                    {
                        List<byte> content = new List<byte>();


                        while ((character = reader.Read()) != -1)
                        {
                            var bytes = Encoding.UTF8.GetBytes(new char[] { (char)character });

                            content.AddRange(bytes);

                            if (content.Count % size100MB == 0)
                            {
                                break;
                            }
                        }

                        if (character == -1)
                        {
                            // Додаємо спочатку 1
                            content.Add(0x80);

                            // Далі додаємо 0
                            for (uint j = 0; j < paddingLength - 1; j++)
                            {
                                content.Add(0x00);
                            }

                            // Додаємо останні 8 байтів (64 біти) - довжину оригінального повідомлення
                            long arrayMessageLengthBits = (long)size * 8;
                            byte[] lengthBytes = BitConverter.GetBytes(arrayMessageLengthBits);

                            content.AddRange(lengthBytes);
                        }
                        
                        if (MD5Result == null)
                        {
                            MD5Result = MD5Algorythm.Algorithm(content.ToArray(), isBigFile: true);
                        }
                        else
                        {
                            var previousResult = new uint[4];
                            Array.Copy(MD5Result, previousResult, MD5Result.Length);

                            MD5Result = MD5Algorythm.Algorithm(content.ToArray(), previousResult, isBigFile: true);
                        }

                        content.Clear();
                    }
                }
            }

            return MD5Result;
        }

        public static ulong CountSizeOfFile(string filePath)
        {
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
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine("Error during reading file: " + ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Error during reading file: " + ex.Message);
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