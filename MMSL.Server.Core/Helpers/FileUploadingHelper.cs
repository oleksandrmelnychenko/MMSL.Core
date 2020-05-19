using Microsoft.AspNetCore.Http;
using MMSL.Common;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Helpers {
    public class FileUploadingHelper {

        private const char urlSeparator = '/';

        public static async Task<string> UploadFile(string baseUrl, IFormFile formFile) {
            string fullFilePath = Path.Combine(ConfigurationManager.UploadsPath, $"{DateTime.Now.Ticks}{Path.GetExtension(formFile.FileName)}");

            using (FileStream stream = File.Create(fullFilePath)) {
                await formFile.CopyToAsync(stream);
            }

            string relatedPath = fullFilePath
                .Replace(ConfigurationManager.ContentRootPath, baseUrl)
                .Replace(Path.DirectorySeparatorChar, urlSeparator);

            return new Uri(relatedPath).AbsoluteUri;
        }

        public static void DeleteFile(string baseUrl, string filePath) {

            string fullFilePath = filePath
                .Replace(baseUrl, ConfigurationManager.ContentRootPath)
                .Replace(urlSeparator, Path.DirectorySeparatorChar);

            if (File.Exists(fullFilePath)) {
                File.Delete(fullFilePath);
            }
        }
    }
}
