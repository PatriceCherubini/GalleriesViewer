using GalerieViewer.Models;
using GalerieViewer.ViewModels;
using System;
using System.Collections.Generic;

namespace GalerieViewer.Services
{
    public interface IGalerieService
    {
        GalerieFullViewModel GenerateGalerie (int id);
        ImageViewModel GetImage(int id);
        void AddImageInGalerie(ImageViewModel img);
        int AddNewGallery(GalerieViewModel gallery);
        void UpdateImage(ImageViewModel img);
        void UpdateGallery(GalerieViewModel gallery);
        void DeleteImage(int idImage, int idGallery);
        void DeleteGallery(int idGallery);
    }
}