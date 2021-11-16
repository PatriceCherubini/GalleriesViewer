using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GalerieViewer.Services
{
    public class PictureUploader : IPictureUploader
    {
        public string ImagePath { get; private set; }
        public IFormFile Image { get; private set; }
        public string UniqueFileName { get; private set; }
        public string SetFileName(IFormFile ImageFile, string name)
        {
            string extension = Path.GetExtension(ImageFile.FileName);
            UniqueFileName = Guid.NewGuid().ToString() + "_" + name + extension;
            Image = ImageFile;
            return UniqueFileName;
        }
        public void SetPath(string root, string folder)
        {
            ImagePath = Path.Combine(root, folder, UniqueFileName);
        }
        public void SetPath(string root)
        {
            ImagePath = Path.Combine(root, UniqueFileName);
        }
        public void Upload()
        {
            using (var fileStream = new FileStream(ImagePath, FileMode.Create))
            {
                Image.CopyTo(fileStream);
            }
        }
    }
}
