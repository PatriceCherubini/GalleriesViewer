using GalerieViewer.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using GalerieViewer.Common;

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
        public GalerieFullViewModel GetPaginatedGallery(int idGallery, int pageSize, int pageNB)
        {
            try
            {
                var gallery = _dataAccess.GetGalerie(idGallery, pageSize);
                gallery = SortGallery(gallery, gallery.SortedBy);
                gallery.ListeImages = gallery.ListeImages
                                     .Skip((pageNB - 1) * pageSize)
                                     .Take(pageSize).ToList();
                return gallery;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public GalerieFullViewModel GetPaginatedGallery(int idGallery, int pageSize, int pageNB, SortType sortedBy)
        {
            try
            {
                var gallery = _dataAccess.UpdateSort(idGallery, pageSize, sortedBy);
                gallery = SortGallery(gallery, sortedBy);
                gallery.ListeImages = gallery.ListeImages
                                     .Skip((pageNB - 1) * pageSize)
                                     .Take(pageSize).ToList();
                return gallery;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public GalerieFullViewModel SortGallery(GalerieFullViewModel Gallery, SortType sortedBy)
        {
            try
            {
                Gallery.ListeImages = Gallery.ListeImages.AsQueryable().OrderBy(sortedBy.ToString()).ToList();
                return Gallery;
            }
            catch (Exception)
            {
                return Gallery;
            }
        }
        /// <summary>
        /// Get a picture to show in a modal. 
        /// </summary>
        /// <param name="idImage">Id of the picture to show</param>
        /// <param name="idGallery">Id of the gallery that contains the picture</param>
        /// <returns>A ViewImageModel to show in a modal</returns>
        public CarouselViewModel ViewCarousel(int idImage, int idGallery)
        {
            CarouselViewModel carousel = _dataAccess.GetCarousel(idGallery, idImage);
            carousel.ListPictures = carousel.ListPictures.AsQueryable().OrderBy(carousel.SortedBy.ToString()).ToList();
            int[] arrayIds = carousel.ListPictures
                                      .Select(i => i.ImageItemId)
                                      .ToArray();
            carousel.Position = Array.IndexOf(arrayIds, idImage);
            return carousel;
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
        /// Add a new picture
        /// </summary>
        /// <param name="img">The ImageViewmodel with every parameters to add a new picture </param>
        public void AddImageInGalerie(ImageViewModel img)
        {
            _dataAccess.AddImage(img, img.GalerieId);
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
            return _dataAccess.AddGallery(gallery);
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
        /// Update a picture
        /// </summary>
        /// <param name="img">The ImageViewmodel with every parameters of the picture to update </param>
        public void UpdateImage(ImageWithoutFileViewModel img)
        {
            _dataAccess.UpdateImage(img);
            _dataAccess.UpdateDateGalery(img.GalerieId);
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
        /// Upload a picture on the server at a specific path
        /// </summary>
        /// <param name="root">name of the root folder</param>
        /// <param name="folder">name of the subfolder</param>
        /// <param name="image">name of the picture</param>
        /// <returns>a unique filename</returns>
        public ImageViewModel UploadImage(string root, string folder, ImageViewModel image)
        {
            _pictureUploader.SetFileName(image.ImageFile, image.Name);
            _pictureUploader.SetPath(root, folder);
            _pictureUploader.Upload(400);
            image.FileName = _pictureUploader.UniqueFileName;
            image.FileNameThumb = _pictureUploader.UniqueFileNameThumb;
            return image;
        }

    }
}
