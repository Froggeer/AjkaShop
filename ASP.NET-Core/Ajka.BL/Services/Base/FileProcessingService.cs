using System.Drawing;
using System.IO;
using Ajka.BL.Services.Base.Interfaces;
using LazZiya.ImageResize;

namespace Ajka.BL.Services.Base
{
    public class FileProcessingService : IFileProcessingService, IAjkaShopService
    {
        public void CreateDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        public void CreateScaledImage(Image image, string path, int width, int height = 0)
        {
            if(height > 0)
            {
                var finalImage = ImageResize.Scale(image, width, height);
                finalImage.SaveAs(path);
            }
            else
            {
                var finalImage = ImageResize.ScaleByWidth(image, width);
                finalImage.SaveAs(path);
            }
        }

        public void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
