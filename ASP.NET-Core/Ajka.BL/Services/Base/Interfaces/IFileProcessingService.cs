using System.Drawing;

namespace Ajka.BL.Services.Base.Interfaces
{
    public interface IFileProcessingService
    {
        void CreateDirectory(string directoryPath);

        void CreateScaledImage(Image image, string path, int width, int height = 0);

        void DeleteFile(string path);
    }
}
