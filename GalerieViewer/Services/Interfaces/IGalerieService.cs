using GalerieViewer.Models;
using GalerieViewer.ViewModels;
using System;
using System.Collections.Generic;

namespace GalerieViewer.Services
{
    public interface IGalerieService
    {
        GalerieFullViewModel GenerateGalerie (int id);
        GalerieFullViewModel GetPaginatedGallery(int idGallery, int pageSize, int pageNB);
        ImageViewModel GetImage(int id);
        ViewImageViewModel ViewImage(int idImage, int idGallery);
        void AddImageInGalerie(ImageViewModel img);
        int AddNewGallery(GalerieViewModel gallery);
        void UpdateImage(ImageViewModel img);
        void UpdateGallery(GalerieViewModel gallery);
        void DeleteImage(int idImage, int idGallery);
        void DeleteGallery(int idGallery);
        string UploadImage(string root, string folder, ImageViewModel image);
    }
}