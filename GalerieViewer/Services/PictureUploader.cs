using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GalerieViewer.Services
{/// <summary>
/// Upload a picture file on a given path
/// </summary>
    public class PictureUploader : IPictureUploader
    {
        public string ImagePath { get; private set; }
        public IFormFile Image { get; private set; }
        public string UniqueFileName { get; private set; }
        /// <summary>
        /// Get a IFormFile and generate return a unique name in string
        /// </summary>
        /// <param name="ImageFile">The picture to upload in IFormatFile </param>
        /// <param name="nameOfFile">the name of the picture, used to generate a unique filename</param>
        /// <returns>A unique name for the picture that will be used as the filename</returns>
        public string SetFileName(IFormFile ImageFile, string nameOfFile)
        {
            string extension = Path.GetExtension(ImageFile.FileName);
            UniqueFileName = Guid.NewGuid().ToString() + "_" + nameOfFile + extension;
            Image = ImageFile;
            return UniqueFileName;
        }/// <summary>
        /// Set the path of the picture
        /// </summary>
        /// <param name="root">name of the root folder</param>
        /// <param name="folder">name of the subfolder</param>
        public void SetPath(string root, string folder)
        {
            ImagePath = Path.Combine(root, folder, UniqueFileName);
        }
        /// <summary>
        /// Set the path of the picture
        /// </summary>
        /// <param name="root">name of the root folder</param>
        public void SetPath(string root)
        {
            ImagePath = Path.Combine(root, UniqueFileName);
        }
        /// <summary>
        /// Set the path of the picture
        /// </summary>
        /// <param name="root">name of the root folder</param>
        /// <param name="folders">array of string with the various subfolders</param>
        public void SetPath(string root, string[] folders)
        {
            string[] newArray = new string[folders.Length + 2];
            newArray[0] = root;
            for (int i = 1; i < newArray.Length - 2; i++)
            {
                newArray[i] = folders[i - 1];
            }
            newArray[newArray.Length-1]  = UniqueFileName;
            ImagePath = Path.Combine(newArray);
        }
        /// <summary>
        /// Upload the Picture in ImagePath
        /// </summary>
        public void Upload()
        {
            using (var fileStream = new FileStream(ImagePath, FileMode.Create))
            {
                Image.CopyTo(fileStream);
            }
        }
    }
}
