using GalerieViewer.Models;
using GalerieViewer.ViewModels;
using System.Collections.Generic;

namespace GalerieViewer.Services
{
    public interface IDataAccess
    {
        void addGalleriesAndImages();
        void UpdateDateGalery(int id);
        List<GalerieViewModel> GetAllGaleries();
        GalerieFullViewModel GetGalerie(int id);
        ImageViewModel GetImage(int id);
        List<ImageViewModel> GetAllImagesItem(int id);
        void AddImageInGalerie(ImageViewModel img, int id);
        int AddNewGallery(GalerieViewModel galerie);
        void DeleteImage(int id);
        void DeleteGallery(int id);
        void DeleteAllImages(int id);
        void UpdateImage(ImageViewModel img);
        void UpdateGallery(GalerieViewModel galerie);
    }
}