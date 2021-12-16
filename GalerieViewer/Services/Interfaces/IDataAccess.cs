using GalerieViewer.Common;
using GalerieViewer.Models;
using GalerieViewer.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GalerieViewer.Services
{
    public interface IDataAccess
    {
        Task<List<GalerieViewModel>> GetAllGaleries(string userID);
        Task<GalerieFullViewModel> GetGalerie(int id, int pageSize);
        Task<GalerieFullViewModel> GetFirstGalerie(string userId, int pageSize);
        Task<CarouselViewModel> GetCarousel(int idGallery, int idImage);
        Task<List<ImageViewModel>> GetAllImagesItem(int id);
        Task<ImageViewModel> GetImage(int id);
        int AddGallery(GalerieViewModel galerie);
        void AddImage(ImageViewModel img, int id);
        void UpdateGallery(GalerieViewModel galerie);
        void UpdateImage(ImageWithoutFileViewModel img);
        void UpdateDateGalery(int id);
        Task<GalerieFullViewModel> UpdateSort(int id, int pageSize, SortType sortedBy);
        void DeleteGallery(int id);
        void DeleteImage(int id);
        void DeleteAllImages(int id);


    }
}