using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using LazZiya.ImageResize;
using System.Drawing;

namespace GalerieViewer.Services
{/// <summary>
/// Upload a picture file on a given path
/// </summary>
    public class PictureUploader : IPictureUploader
    {
        public string ImagePath { get; private set; }
        public string ImagePathThumb { get; private set; }
        public IFormFile ImageFile { get; private set; }
        public string UniqueFileName { get; private set; }
        public string UniqueFileNameThumb { get; private set; }
        /// <summary>
        /// Get a IFormFile and generate return a unique name in string
        /// </summary>
        /// <param name="ImageFile">The picture to upload in IFormatFile </param>
        /// <param name="nameOfFile">the name of the picture, used to generate a unique filename</param>
        /// <returns>A unique name for the picture that will be used as the filename</returns>
        public void SetFileName(IFormFile imageFile, string nameOfFile)
        {
            string extension = Path.GetExtension(imageFile.FileName);
            string fileNameNoExt = Guid.NewGuid().ToString() + "_" + nameOfFile;
            UniqueFileName = fileNameNoExt + extension;
            UniqueFileNameThumb = fileNameNoExt + "_thumb" + extension;
            ImageFile = imageFile;
        }
        /// <summary>
        /// Set the path of the picture
        /// </summary>
        /// <param name="root">name of the root folder</param>
        /// <param name="folder">name of the subfolder</param>
        public void SetPath(string root, string folder)
        {
            ImagePath = Path.Combine(root, folder, UniqueFileName);
            ImagePathThumb = Path.Combine(root, folder, UniqueFileNameThumb);
        }
        /// <summary>
        /// Set the path of the picture
        /// </summary>
        /// <param name="root">name of the root folder</param>
        public void SetPath(string root)
        {
            ImagePath = Path.Combine(root, UniqueFileName);
            ImagePathThumb = Path.Combine(root, UniqueFileNameThumb);
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
            newArray[newArray.Length - 1] = UniqueFileNameThumb;
            ImagePathThumb = Path.Combine(newArray);
        }
        /// <summary>
        /// Upload the Picture in ImagePath
        /// </summary>
        public void Upload(int thumbSize)
        {
            using (var fileStream = new FileStream(ImagePath, FileMode.Create))
            {
                ImageFile.CopyTo(fileStream);
            }
            // Create a thumbnail of the picture 
            using (var img = Image.FromFile(ImagePath))
            {
                img.ScaleByWidth(thumbSize)
                    .SaveAs(ImagePathThumb);
            }
        }
    }
}
