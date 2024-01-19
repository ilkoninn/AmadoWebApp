using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Business.Helpers
{
    public static class FileManager
    {
        public static bool CheckImage(this IFormFile file)
        {
            return file.ContentType.Contains("image/") && file.Length / 1024 / 1024 <= 3;
        }
        public static string Upload(this IFormFile file, string webRoot, string folderName)
        {
            if (!Directory.Exists(webRoot + folderName))
            {
                Directory.CreateDirectory(webRoot + folderName);
            }

            string fileName = Guid.NewGuid().ToString() + file.FileName;

            string filePath = webRoot + folderName + fileName;

            using (FileStream st = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(st);
            }

            return fileName;
        }

        public static void Delete(string fileName, string webRoot, string folderName)
        {
            if (File.Exists(webRoot + folderName + fileName))
            {
                File.Delete(webRoot + folderName + fileName);
            }
        }
    }
}
