using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TaxshilaMobile
{
    public static class FileExtensions
    {
        private static readonly string LocalFolder, LocalAttachmentFolder;

        static FileExtensions()
        {
            LocalFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            LocalAttachmentFolder = Path.Combine(LocalFolder, "Library", "Attachment");

            if (!Directory.Exists(LocalAttachmentFolder))
            {
                Directory.CreateDirectory(LocalAttachmentFolder);
            }

            //    LocalFolder= Path.Combine(libFolder, filename);
            // Gets the target platform's valid save location

        }

        // Byte[] extension methods

        public static string SaveToLocalFolder(this byte[] dataBytes, string fileName, string extension)
        {
            var filePath = Path.Combine(LocalFolder, $"{fileName}.{extension}");
            if (File.Exists(filePath))
            {
                var fileCount = Directory.GetFiles(LocalFolder).Count();
                filePath = Path.Combine(LocalFolder, $"{fileName}_{fileCount}.{extension}");
            }

            File.WriteAllBytes(filePath, dataBytes);
            return filePath;
        }
        public static string SaveAttachmentInLocalFolder(this byte[] dataBytes, string fileName, string extension)
        {
            var filePath = Path.Combine(LocalAttachmentFolder, $"{fileName}.{extension}");
            if (File.Exists(filePath))
            {
                var fileCount = Directory.GetFiles(LocalAttachmentFolder).Count();
                filePath = Path.Combine(LocalAttachmentFolder, $"{fileName}_{fileCount}.{extension}");
            }

            File.WriteAllBytes(filePath, dataBytes);
            return filePath;
        }
        public static async Task<byte[]> LoadFileBytesAsync(string filePath)
        {
            return File.ReadAllBytes(filePath);
            //return await Task.Run(() => File.ReadAllBytes(filePath));
        }

        public static void DeleteFile(string filePath)
        {
            // Use Combine so that the correct file path slashes are used
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        // Stream extension methods

        public static async Task<string> SaveToLocalFolderAsync(this Stream dataStream, string fileName)
        {
            // Use Combine so that the correct file path slashes are used
            var filePath = Path.Combine(LocalFolder, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

            using (var fileStream = File.OpenWrite(filePath))
            {
                if (dataStream.CanSeek)
                    dataStream.Position = 0;

                await dataStream.CopyToAsync(fileStream);

                return filePath;
            }
        }

        public static async Task<string> DownloadFileandSaveInLocalfolderAsync(string Fileurl,string fileName)
        {
            Uri url = new Uri(Fileurl);
            var client = new HttpClient();
            var httpResponse = await client.GetAsync(url);
            byte[] dataBuffer = await httpResponse.Content.ReadAsByteArrayAsync();
            Stream dataStream = new MemoryStream(dataBuffer);

            // Use Combine so that the correct file path slashes are used
            var filePath = Path.Combine(LocalFolder, fileName);

            if (File.Exists(filePath))
                return filePath;

            using (var fileStream = File.OpenWrite(filePath))
            {
                if (dataStream.CanSeek)
                    dataStream.Position = 0;

                await dataStream.CopyToAsync(fileStream);

                return filePath;
            }
        }

       

        public static async Task<string> IsFileExistinLocal(string fileName)
        {
            var filePath = Path.Combine(LocalFolder, fileName);
            if (File.Exists(filePath))
                return filePath;
            else
                return string.Empty;
        }

        public static async Task<Stream> LoadFileStreamAsync(string filePath)
        {
            return await Task.Run(() =>
            {
                using (var fileStream = File.OpenRead(filePath))
                {
                    return fileStream;
                }
            });
        }
    }
}
