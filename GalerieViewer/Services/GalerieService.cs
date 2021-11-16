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
    public class GalerieService : IGalerieService
    {
        private IDataAccess _dataAccess;
        public GalerieService(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }
        public GalerieFullViewModel GenerateGalerie(int id)
        {
            try
            {
                var gallery = _dataAccess.GetGalerie(id);
                return gallery;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public void AddImageInGalerie(ImageViewModel img)
        {
            _dataAccess.AddImageInGalerie(img, img.GalerieId);
            _dataAccess.UpdateDateGalery(img.GalerieId);
        }
        public int AddNewGallery(GalerieViewModel gallery)
        {
            return _dataAccess.AddNewGallery(gallery);
        }
        public void DeleteImage(int idImage, int IdGallery)
        {
            _dataAccess.DeleteImage(idImage);
            _dataAccess.UpdateDateGalery(IdGallery);
        }
        public void DeleteGallery(int IdGallery)
        {
            _dataAccess.DeleteGallery(IdGallery);
            _dataAccess.UpdateDateGalery(IdGallery);
        }

        public ImageViewModel GetImage(int id)
        {
            return _dataAccess.GetImage(id);
        }

        public void UpdateImage(ImageViewModel img)
        {
            _dataAccess.UpdateImage(img);
           _dataAccess.UpdateDateGalery(img.GalerieId);
        }
        public void UpdateGallery(GalerieViewModel gallery)
        {
            _dataAccess.UpdateGallery(gallery);
        }
    }
}
