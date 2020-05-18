using Microsoft.AspNetCore.Http;
using MMSL.Common;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MMSL.Server.Core.Helpers {
    public class FileUploadingHelper {
        public static async Task<string> UploadFile(IFormFile formFile) {
            string fullFilePath = Path.Combine(ConfigurationManager.UploadsPath, $"{DateTime.Now.Ticks}{Path.GetExtension(formFile.FileName)}");

            using (var stream = File.Create(fullFilePath)) {
                await formFile.CopyToAsync(stream);
            }

            return fullFilePath.Replace($"{ConfigurationManager.ContentRootPath}{Path.DirectorySeparatorChar}", string.Empty);
        }

        public static void DeleteFile(string filePath) {
            string fullFilePath = Path.Combine(ConfigurationManager.ContentRootPath, filePath);

            if (File.Exists(fullFilePath)) {
                File.Delete(fullFilePath);
            }
        }
    }
}
