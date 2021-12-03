using GalerieViewer.Common;
using GalerieViewer.ViewModels;
using System;
using System.Collections.Generic;

namespace GalerieViewer.Services
{
    public interface IGalerieService
    {
        GalerieFullViewModel GetPaginatedGallery(int idGallery, int pageSize, int pageNB);
        GalerieFullViewModel GetPaginatedGallery(int idGallery, int pageSize, int PageNB, SortType SortedBy);
        GalerieFullViewModel SortGallery(GalerieFullViewModel Gallery, SortType SortedBy);
        CarouselViewModel ViewCarousel(int idImage, int idGallery);
        ImageViewModel GetImage(int id);
        int AddNewGallery(GalerieViewModel gallery);
        void AddImageInGalerie(ImageViewModel img);
        void UpdateGallery(GalerieViewModel gallery);
        void UpdateImage(ImageWithoutFileViewModel img);
        void DeleteGallery(int idGallery);
        void DeleteImage(int idImage, int idGallery);
        ImageViewModel UploadImage(string root, string folder, ImageViewModel image);
    }
}