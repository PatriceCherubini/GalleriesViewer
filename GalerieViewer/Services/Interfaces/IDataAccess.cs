using GalerieViewer.Models;
using GalerieViewer.ViewModels;
using System.Collections.Generic;

namespace GalerieViewer.Services
{
    public interface IDataAccess
    {
        List<GalerieViewModel> GetAllGaleries();
        GalerieFullViewModel GetGalerie(int id);
        GalerieFullViewModel GetPaginatedGallery(int idGallery, int pageSize, int PageNB);
        void UpdateDateGalery(int id);
        List<ImageViewModel> GetAllImagesItem(int id);
        ImageViewModel GetImage(int id);
        CarouselViewModel GetCarousel(int idGallery, int idImage);
        int[] GetListIdsImages(int idGallery);
        int AddGallery(GalerieViewModel galerie);
        void AddImage(ImageViewModel img, int id);
        void DeleteImage(int id);
        void DeleteGallery(int id);
        void DeleteAllImages(int id);
        void UpdateImage(ImageViewModel img);
        void UpdateGallery(GalerieViewModel galerie);
    }
}