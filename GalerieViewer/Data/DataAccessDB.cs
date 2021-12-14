using GalerieViewer.Common;
using GalerieViewer.Models;
using GalerieViewer.Services;
using GalerieViewer.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalerieViewer.Data
{
    /// <summary>
    /// Interract with the context to manage persistence of datas
    /// </summary>
    public class DataAccessDB : IDataAccess
    {
        private readonly AppDbContext _context;
        public DataAccessDB(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get a list of all galleriers
        /// </summary>
        /// <returns>A List containing all galleries in GalerieViewModel format</returns>
        public async Task<List<GalerieViewModel>> GetAllGaleries()
        {
            return await _context.Galeries.Where(x => x.IsDeleted == false)
                                    .Include("ImageItems")
                                    .Select(g => new GalerieViewModel()
                                    {
                                        Id = g.GalerieId,
                                        Name = g.Nom,
                                        DateCreation = g.DateCreation,
                                        DateUpdate = g.DateUpdate,
                                        Description = g.Description,
                                        nbImageItems = g.ImageItems.Where(i => i.IsDeleted == false).Count(),
                                        TotalPages = 0
                                    })
                                   .ToListAsync();
        }
        /// <summary>
        /// Look for a specific gallery. If not found, get the first gallery (smallest if). If still no gallery is found, throw an exception
        /// </summary>
        /// <param name="id">The id of the gallery</param>
        /// <returns>A gallery (GalerieFullViewModel) with all pictures (if any) in it</returns>
        public async Task<GalerieFullViewModel> GetGalerie(int id, int pageSize)
        {

            var galerie = await _context.Galeries.Where(g => g.GalerieId == id && g.IsDeleted == false)
                                           .Include("ImageItems")
                                           .FirstOrDefaultAsync();
            if (galerie == null)
            {
                galerie = await GetFirstGalerie();
                if (galerie == null)
                {
                    throw new Exception("No gallery found");
                }
            }

            int nbImages = galerie.ImageItems.Where(i => i.IsDeleted == false).Count();

            return new GalerieFullViewModel()
            {
                Id = galerie.GalerieId,
                Name = galerie.Nom,
                DateCreation = galerie.DateCreation,
                DateUpdate = galerie.DateUpdate,
                Description = galerie.Description,
                SortedBy = galerie.SortedBy,
                nbImageItems = nbImages,
                TotalPages = (int)Math.Ceiling(decimal.Divide(nbImages, pageSize)),
                ListeImages = await GetAllImagesItem(galerie.GalerieId)
            };

        }
        /// <summary>
        /// Method that return the gallery with the smallest id in the context
        /// </summary>
        /// <returns>The gallery (GalerieFullViewModel) with the smallest id</returns>
        private async Task<Galerie> GetFirstGalerie()
        {
            return await _context.Galeries.Where(x => x.IsDeleted == false)
                                    .OrderBy(g => g.GalerieId)
                                    .Include("ImageItems")
                                    .FirstOrDefaultAsync();
        }
        public async Task<CarouselViewModel> GetCarousel(int idGallery, int idImage)
        {
            SortType sortedBy = _context.Galeries.Where(g => g.GalerieId == idGallery && g.IsDeleted == false)
                                                 .Select(s => s.SortedBy)
                                                 .FirstOrDefault();

            var listgalleries = await GetAllImagesItem(idGallery);
            return new CarouselViewModel { ListPictures = listgalleries, SortedBy = sortedBy };
        }

        /// <summary>
        /// Get all pictures from a specific gallery 
        /// </summary>
        /// <param name="id">id the gallery</param>
        /// <returns>A list of all the picture from a gallery, in ImageViewModel format</returns>
        public async Task<List<ImageViewModel>> GetAllImagesItem(int id)
        {           
                return await _context.ImageItems.Where(a => a.GalerieId == id)
                                                .Where(i => !i.IsDeleted)
                                                .Select(i => new ImageViewModel()
                                                {
                                                    ImageItemId = i.ImageItemId,
                                                    GalerieId = i.GalerieId,
                                                    Name = i.Name,
                                                    DateCreation = i.DateCreation,
                                                    DateUpload = i.DateUpload,
                                                    Description = i.Description,
                                                    FileName = i.FileName,
                                                    FileNameThumb = i.FileNameThumb
                                                })
                                                .ToListAsync();
                                  
        }
        /// <summary>
        /// Get a picture a return it in ImageViewModel format
        /// </summary>
        /// <param name="id">id of the picture</param>
        /// <returns>A picture in ImageViewModel format</returns>
        public async Task<ImageViewModel> GetImage(int id)
        {
            return await _context.ImageItems.Where(i => i.ImageItemId == id)
                                      .Select(i => new ImageViewModel()
                                      {
                                          ImageItemId = i.ImageItemId,
                                          GalerieId = i.GalerieId,
                                          Name = i.Name,
                                          DateCreation = i.DateCreation,
                                          DateUpload = i.DateUpload,
                                          Description = i.Description,
                                          FileName = i.FileName,
                                          FileNameThumb = i.FileNameThumb
                                      })
                                      .FirstOrDefaultAsync();
        }
        /// <summary>
        /// Add a new gallery in the context
        /// </summary>
        /// <param name="galerie">the gallery in GalerieViewModel format</param>
        /// <returns>The newly generated id for the gallery</returns>
        public int AddGallery(GalerieViewModel galerie)
        {
            var newGalerie = new Galerie()
            {
                Nom = galerie.Name,
                DateCreation = DateTime.Now,
                DateUpdate = DateTime.Now,
                Description = galerie.Description,
                SortedBy = SortType.DateCreation,
                ImageItems = new List<ImageItem>()
            };
            _context.Galeries.Add(newGalerie);
            _context.SaveChanges();

            return newGalerie.GalerieId;
        }
       
        /// <summary>
        /// Delete a gallery in the context => change isDeleted to "true"
        /// </summary>
        /// <param name="id">Id of the gallery</param>
        public void DeleteGallery(int id)
        {
            _context.Galeries.Where(x => x.GalerieId == id)
                               .FirstOrDefault()
                               .IsDeleted = true;
            // Delete all pictures form the gallery
            DeleteAllImages(id);

            _context.SaveChanges();
        }

        /// <summary>
        /// Udpate the dateUpdate of a gallery with current DateTime
        /// </summary>
        /// <param name="id">Id of the gallery</param>
        public void UpdateDateGalery(int id)
        {
            _context.Galeries.Where(g => g.GalerieId == id)
                             .FirstOrDefault()
                             .DateUpdate = DateTime.Now;

            _context.SaveChanges();
        }

        /// <summary>
        /// Update a gallery
        /// </summary>
        /// <param name="galerie">The gallery to update in GalerieViewModel format</param>
        public void UpdateGallery(GalerieViewModel galerie)
        {
            var updatedGallery = _context.Galeries.Where(i => i.GalerieId == galerie.Id).FirstOrDefault();
            updatedGallery.Description = galerie.Description;
            updatedGallery.Nom = galerie.Name;
            updatedGallery.DateUpdate = DateTime.Now;

            _context.SaveChanges();
        }

        public async Task<GalerieFullViewModel> UpdateSort(int id, int pageSize, SortType sortedBy)
        {
            var updatedGallery = _context.Galeries.Where(i => i.GalerieId == id).FirstOrDefault();
            updatedGallery.SortedBy = sortedBy;

            _context.SaveChanges();

            return await GetGalerie(id, pageSize);
        }

        /// <summary>
        /// Add a new picture in a gallery (and in the context)
        /// </summary>
        /// <param name="img">The picture in ImageViewModel format</param>
        /// <param name="id">The Id of the gallery that will contains the new picture </param>
        public void AddImage(ImageViewModel img, int id)
        {
            var galerie = _context.Galeries.Where(x => x.GalerieId == id)
                             .Include("ImageItems")
                             .FirstOrDefault();

            galerie.DateUpdate = DateTime.Now;
            galerie.ImageItems.Add(new ImageItem()
            {
                Name = img.Name,
                Description = img.Description,
                DateCreation = img.DateCreation,
                DateUpload = DateTime.Now,
                FileName = img.FileName,
                FileNameThumb = img.FileNameThumb
            });

            _context.SaveChanges();
        }
        /// <summary>
        /// Delete a picture in the context => change isDeleted to "true"
        /// </summary>
        /// <param name="id">Id of the picture</param>
        public void DeleteImage(int id)
        {
            _context.ImageItems.Where(x => x.ImageItemId == id)
                               .FirstOrDefault()
                               .IsDeleted = true;

            _context.SaveChanges();
        }
    
        /// <summary>
        /// Delete all pictures form a gallery in the context => change isDeleted to "true"
        /// </summary>
        /// <param name="id">id of the gallery</param>
        public void DeleteAllImages(int id)
        {
            foreach (var item in _context.ImageItems.Where(x => x.GalerieId == id))
            {
                item.IsDeleted = true;
            }
        }

        /// <summary>
        /// Update a picture
        /// </summary>
        /// <param name="img">The picture to update in ImageViewModel format</param>
        public void UpdateImage(ImageWithoutFileViewModel img)
        {
            var updatedImg = _context.ImageItems.Where(i => i.ImageItemId == img.ImageItemId).FirstOrDefault();
            updatedImg.DateCreation = img.DateCreation;
            updatedImg.Description = img.Description;
            updatedImg.Name = img.Name;
            updatedImg.DateUpload = DateTime.Now;

            _context.SaveChanges();
        }
    }
}
