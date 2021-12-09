using Microsoft.AspNetCore.Http;

namespace GalerieViewer.Services
{
    public interface IPictureUploader
    {
        IFormFile ImageFile { get; }
        string ImagePath { get; }
        string UniqueFileName { get; }
        string UniqueFileNameThumb { get; }
        void SetFileName(IFormFile ImageFile, string name);
        void SetPath(string root);
        void SetPath(string root, string folder);
        void Upload(int sizeThumb);
    }
}