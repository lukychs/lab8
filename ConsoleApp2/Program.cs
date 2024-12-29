using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace TestConcole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string catalog = @"C:\Users\Alex\Source\Repos\lab8_c#\ConsoleApp2\bin\Debug";
            string fileName = "text.txt";

            //проводим поиск в выбранном каталоге и во всех его подкаталогах
            foreach (string findedFile in Directory.EnumerateFiles(catalog, fileName,
                SearchOption.AllDirectories))
            {
                FileInfo FI;
                try
                {
                    //по полному пути к файлу создаём объект класса FileInfo
                    FI = new FileInfo(findedFile);
                    //найденный результат выводим в консоль (имя, путь, размер, дата создания файла)

                    Console.WriteLine($"Полный путь к файлу {fileName}: {FI.FullName}");

                    using (FileStream fs = new FileStream(FI.FullName, FileMode.Open, FileAccess.Read))
                    {
                        byte[] data = new byte[fs.Length];
                        int numBytesToRead = (int)fs.Length;
                        int numBytesRead = 0;

                        while (numBytesToRead > 0)
                        {
                            int n = fs.Read(data, numBytesRead, numBytesToRead);

                            if (n == 0)
                                break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }

                        string text = Encoding.UTF8.GetString(data);
                        Console.WriteLine($"Содержимое файла {fileName}:");
                        Console.WriteLine(text);
                    }

                    string sourceFile = "text.txt"; // исходный файл
                    string compressedFile = "text.gz"; // сжатый файл
                    string targetFile = "text_new.txt"; // восстановленный файл

                    // создание сжатого файла
                    await CompressAsync(sourceFile, compressedFile);
                    // чтение из сжатого файла
                    await DecompressAsync(compressedFile, targetFile);

                    async Task CompressAsync(string sourceFile, string compressedFile)
                    {
                        // поток для чтения исходного файла
                        using FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate);
                        // поток для записи сжатого файла
                        using FileStream targetStream = File.Create(compressedFile);

                        // поток архивации
                        using GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress);
                        await sourceStream.CopyToAsync(compressionStream); // копируем байты из одного потока в другой

                        Console.WriteLine($"Сжатие файла {sourceFile} завершено");
                        Console.WriteLine($"Исходный размер: {sourceStream.Length}  сжатый размер: {targetStream.Length}");
                    }

                    async Task DecompressAsync(string compressedFile, string targetFile)
                    {
                        // поток для чтения из сжатого файла
                        using FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate);
                        // поток для записи восстановленного файла
                        using FileStream targetStream = File.Create(targetFile);
                        // поток разархивации
                        using GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress);
                        await decompressionStream.CopyToAsync(targetStream);
                        Console.WriteLine($"Восстановлен файл: {targetFile}");
                    }
                }

                catch 
                {
                    continue;
                }

            }
        }

    }
}