using GalerieViewer.Data;
using GalerieViewer.Models;
using GalerieViewer.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalerieViewer.Services
{
    /// <summary>
    /// Service with everything you need to manage a gallery
    /// </summary>
    public class GalerieService : IGalerieService
    {
        private IDataAccess _dataAccess;
        private readonly IPictureUploader _pictureUploader;
        public GalerieService(IDataAccess dataAccess, IPictureUploader pictureUploader)
        {
            _dataAccess = dataAccess;
            _pictureUploader = pictureUploader;
        }
        /// <summary>
        /// Get a gallery with the specific id
        /// </summary>
        /// <param name="id">id of the gallery</param>
        /// <returns>Either the specific gallery, or if there are no gallery</returns>
        public GalerieFullViewModel GenerateGalerie(int id)
        {
            try
            {
                var gallery = _dataAccess.GetGalerie(id);
                return gallery;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Get a picture to show in a modal. 
        /// </summary>
        /// <param name="idImage">Id of the picture to show</param>
        /// <param name="idGallery">Id of the gallery that contains the picture</param>
        /// <returns>A ViewImageModel to show in a modal</returns>
        public ViewImageViewModel ViewImage (int idImage, int idGallery)
        {
            int[] arrayIds = _dataAccess.GetListIdsImages(idGallery);
            int positionImageinGallery = Array.IndexOf(arrayIds, idImage);
            int? previous = (positionImageinGallery > 0) ? arrayIds[positionImageinGallery - 1] : null;
            int? next = (positionImageinGallery < arrayIds.Length - 1) ? arrayIds[positionImageinGallery + 1] : null;
           
           return new ViewImageViewModel { Image = GetImage(idImage), IdNext = next, IdPrevious = previous, Position = $"Picture {positionImageinGallery + 1} of {arrayIds.Length}" };
        }
        /// <summary>
        /// Add a new picture
        /// </summary>
        /// <param name="img">The ImageViewmodel with every parameters to add a new picture </param>
        public void AddImageInGalerie(ImageViewModel img)
        {
            _dataAccess.AddImageInGalerie(img, img.GalerieId);
            // Update the last Update date from the gallery where the new picture is added 
            _dataAccess.UpdateDateGalery(img.GalerieId);
        }
        /// <summary>
        /// Add a new gallery
        /// </summary>
        /// <param name="gallery">The GalerieViewModel with every parameters to add a new gallery</param>
        /// <returns></returns>
        public int AddNewGallery(GalerieViewModel gallery)
        {
            return _dataAccess.AddNewGallery(gallery);
        }
        /// <summary>
        /// Delete a picture
        /// </summary>
        /// <param name="idImage">Id of the picture</param>
        /// <param name="IdGallery">Id of the gallery that contains the picture</param>
        public void DeleteImage(int idImage, int IdGallery)
        {
            _dataAccess.DeleteImage(idImage);
            // Update the last Update date from the gallery that contains the picture
            _dataAccess.UpdateDateGalery(IdGallery);
        }
        /// <summary>
        /// Delete a gallery
        /// </summary>
        /// <param name="IdGallery">id of the gallery</param>
        public void DeleteGallery(int IdGallery)
        {
            _dataAccess.DeleteGallery(IdGallery);
            _dataAccess.UpdateDateGalery(IdGallery);
        }
        /// <summary>
        /// Get a picture
        /// </summary>
        /// <param name="id">the id of the picture</param>
        /// <returns></returns>
        public ImageViewModel GetImage(int id)
        {
            return _dataAccess.GetImage(id);
        }
        /// <summary>
        /// Update a picture
        /// </summary>
        /// <param name="img">The ImageViewmodel with every parameters of the picture to update </param>
        public void UpdateImage(ImageViewModel img)
        {
            _dataAccess.UpdateImage(img);
            _dataAccess.UpdateDateGalery(img.GalerieId);
        }
        /// <summary>
        /// Update a gallery
        /// </summary>
        /// <param name="gallery">The GalerieViewModel with every parameters of the gallery to update</param>
        public void UpdateGallery(GalerieViewModel gallery)
        {
            _dataAccess.UpdateGallery(gallery);
        }
        /// <summary>
        /// Upload a picture on the server at a specific path
        /// </summary>
        /// <param name="root">name of the root folder</param>
        /// <param name="folder">name of the subfolder</param>
        /// <param name="image">name of the picture</param>
        /// <returns>a unique filename</returns>
        public string UploadImage(string root, string folder, ImageViewModel image)
        {
            string uniqueFileName = _pictureUploader.SetFileName(image.ImageFile, image.Nom);
            _pictureUploader.SetPath(root, folder);
            _pictureUploader.Upload();

            return uniqueFileName;
        }

        public GalerieFullViewModel GetPaginatedGallery(int idGallery, int pageSize, int pageNB)
        {
            return _dataAccess.GetPaginatedGallery(idGallery, pageSize, pageNB);
        }
    }
}
