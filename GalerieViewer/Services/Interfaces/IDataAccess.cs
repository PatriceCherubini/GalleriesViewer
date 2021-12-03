using GalerieViewer.Common;
using GalerieViewer.Models;
using GalerieViewer.ViewModels;
using System.Collections.Generic;

namespace GalerieViewer.Services
{
    public interface IDataAccess
    {
        List<GalerieViewModel> GetAllGaleries();
        GalerieFullViewModel GetGalerie(int id, int pageSize);
        CarouselViewModel GetCarousel(int idGallery, int idImage);
        List<ImageViewModel> GetAllImagesItem(int id);
        ImageViewModel GetImage(int id);
        int AddGallery(GalerieViewModel galerie);
        void AddImage(ImageViewModel img, int id);
        void UpdateGallery(GalerieViewModel galerie);
        void UpdateImage(ImageWithoutFileViewModel img);
        void UpdateDateGalery(int id);
        GalerieFullViewModel UpdateSort(int id, int pageSize, SortType sortedBy);
        void DeleteGallery(int id);
        void DeleteImage(int id);
        void DeleteAllImages(int id);


    }
}