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
        private AppDbContext _context;
        public DataAccessDB(AppDbContext context)
        {
            _context = context;
        }
        public void addGalleriesAndImages()
        {
            Galerie _gal = new Galerie { Nom = "Galerie Une", DateCreation = new DateTime(2020, 12, 12, 7, 47, 0), Description = "Ceci est une jolie galerie de châteaux médéviaux", DateUpdate = DateTime.Now };
            ImageItem _img10 = new ImageItem { Nom = $"Image10", DateCreation = new DateTime(2020, 12, 12, 7, 47, 0), DateUpload = DateTime.Now };
            ImageItem _img11 = new ImageItem { Nom = $"Image11", DateCreation = new DateTime(2020, 12, 12, 7, 47, 0), DateUpload = DateTime.Now };
            List<ImageItem> _list1 = new List<ImageItem>();
            _list1.Add(_img10);
            _list1.Add(_img11);
            _gal.ImageItems = _list1;

            Galerie _gal2 = new Galerie { Nom = "Galerie Deux", DateCreation = new DateTime(2020, 12, 12, 7, 47, 0), Description = "Ceci est une jolie galerie de châteaux médéviaux", DateUpdate = DateTime.Now };
            ImageItem _img20 = new ImageItem { Nom = $"Image20", DateCreation = new DateTime(2020, 12, 12, 7, 47, 0), DateUpload = DateTime.Now };
            ImageItem _img21 = new ImageItem { Nom = $"Image21", DateCreation = new DateTime(2020, 12, 12, 7, 47, 0), DateUpload = DateTime.Now };
            List<ImageItem> _list2 = new List<ImageItem>();
            _list2.Add(_img20);
            _list2.Add(_img21);
            _gal2.ImageItems = _list2;

            Galerie _gal3 = new Galerie { Nom = "Galerie Trois", DateCreation = new DateTime(2020, 12, 12, 7, 47, 0), Description = "Ceci est une jolie galerie de châteaux médéviaux", DateUpdate = DateTime.Now };
            ImageItem _img30 = new ImageItem { Nom = $"Image30", DateCreation = new DateTime(2020, 12, 12, 7, 47, 0), DateUpload = DateTime.Now };
            ImageItem _img31 = new ImageItem { Nom = $"Image31", DateCreation = new DateTime(2020, 12, 12, 7, 47, 0), DateUpload = DateTime.Now };
            List<ImageItem> _list3 = new List<ImageItem>();
            _list3.Add(_img30);
            _list3.Add(_img31);
            _gal3.ImageItems = _list3;

            _context.Add(_gal);
            _context.Add(_gal2);
            _context.Add(_gal3);
            _context.SaveChanges();
        }

        /// <summary>
        /// Get a list of all galleriers
        /// </summary>
        /// <returns>A List containing all galleries in GalerieViewModel format</returns>
        public List<GalerieViewModel> GetAllGaleries()
        {

            return _context.Galeries.Where(x => x.IsDeleted == false)
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
                                   .ToList();
        }
        /// <summary>
        /// Look for a specific gallery. If not found, get the first gallery (smallest if). If still no gallery is found, throw an exception
        /// </summary>
        /// <param name="id">The id of the gallery</param>
        /// <returns>A gallery (GalerieFullViewModel) with all pictures (if any) in it</returns>
        public GalerieFullViewModel GetGalerie(int id)
        {

            var galerie = _context.Galeries.Where(g => g.GalerieId == id && g.IsDeleted == false)
                                           .FirstOrDefault();
            if (galerie == null)
            {
                galerie = GetFirstGalerie();
                if (galerie == null)
                {
                    throw new Exception("No gallery found");
                }
            }

            return new GalerieFullViewModel()
            {
                Id = galerie.GalerieId,
                Name = galerie.Nom,
                DateCreation = galerie.DateCreation,
                DateUpdate = galerie.DateUpdate,
                Description = galerie.Description,
                ListeImages = GetAllImagesItem(galerie.GalerieId)
            };

        }
        /// <summary>
        /// Method that return the gallery with the smallest id in the context
        /// </summary>
        /// <returns>The gallery (GalerieFullViewModel) with the smallest id</returns>
        private Galerie GetFirstGalerie()
        {
            return _context.Galeries.Where(x => x.IsDeleted == false)
                                    .OrderBy(g => g.GalerieId)
                                    .Include("ImageItems")
                                    .FirstOrDefault();
        }
        /// <summary>
        /// Get all pictures from a specific gallery 
        /// </summary>
        /// <param name="id">id the gallery</param>
        /// <returns>A list of all the picture from a gallery, in ImageViewModel format</returns>
        public List<ImageViewModel> GetAllImagesItem(int id)
        {
            return _context.Galeries.Where(a => a.GalerieId == id)
                                    .Include("ImageItems")
                                    .FirstOrDefault()
                                    .ImageItems
                                    .Where(i => !i.IsDeleted)
                                    .Select(i => new ImageViewModel()
                                    {
                                        ImageItemId = i.ImageItemId,
                                        GalerieId = i.GalerieId,
                                        Nom = i.Nom,
                                        DateCreation = i.DateCreation,
                                        Description = i.Description,
                                        FileName = i.FileName
                                    })
                                    .ToList();
        }
        public List<ImageViewModel> GetPaginatedImagesItem(int id, int pageSize, int pageNB)
        {
            return _context.Galeries.Where(a => a.GalerieId == id)
                                    .Include("ImageItems")
                                    .FirstOrDefault()
                                    .ImageItems
                                    .Where(i => !i.IsDeleted)
                                    .OrderBy(i => i.ImageItemId)
                                    .Skip((pageNB - 1) * pageSize)
                                    .Take(pageSize)
                                    .Select(i => new ImageViewModel()
                                    {
                                        ImageItemId = i.ImageItemId,
                                        GalerieId = i.GalerieId,
                                        Nom = i.Nom,
                                        DateCreation = i.DateCreation,
                                        Description = i.Description,
                                        FileName = i.FileName
                                    }).ToList();
        }


        public int[] GetListIdsImages(int idGallery)
        {
            return _context.ImageItems.Where(i => i.IsDeleted == false && i.GalerieId == idGallery)
                                      .Select(i => i.ImageItemId)
                                      .ToArray();
        }
        /// <summary>
        /// Add a new picture in a gallery (and in the context)
        /// </summary>
        /// <param name="img">The picture in ImageViewModel format</param>
        /// <param name="id">The Id of the gallery that will contains the new picture </param>
        public void AddImageInGalerie(ImageViewModel img, int id)
        {
            var galerie = _context.Galeries.Where(x => x.GalerieId == id)
                             .Include("ImageItems")
                             .FirstOrDefault();

            galerie.DateUpdate = DateTime.Now;
            galerie.ImageItems.Add(new ImageItem()
            {
                Nom = img.Nom,
                Description = img.Description,
                DateCreation = img.DateCreation,
                DateUpload = DateTime.Now,
                FileName = img.FileName
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
        /// Add a new gallery in the context
        /// </summary>
        /// <param name="galerie">the gallery in GalerieViewModel format</param>
        /// <returns>The newly generated id for the gallery</returns>
        public int AddNewGallery(GalerieViewModel galerie)
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
        /// Get a picture a return it in ImageViewModel format
        /// </summary>
        /// <param name="id">id of the picture</param>
        /// <returns>A picture in ImageViewModel format</returns>
        public ImageViewModel GetImage(int id)
        {
            return _context.ImageItems.Where(i => i.ImageItemId == id)
                                      .Select(i => new ImageViewModel()
                                      {
                                          ImageItemId = i.ImageItemId,
                                          GalerieId = i.GalerieId,
                                          Nom = i.Nom,
                                          DateCreation = i.DateCreation,
                                          Description = i.Description,
                                          FileName = i.FileName
                                      })
                                      .FirstOrDefault();
        }
        /// <summary>
        /// Update a picture
        /// </summary>
        /// <param name="img">The picture to update in ImageViewModel format</param>
        public void UpdateImage(ImageViewModel img)
        {
            var updatedImg = _context.ImageItems.Where(i => i.ImageItemId == img.ImageItemId).FirstOrDefault();
            updatedImg.DateCreation = img.DateCreation;
            updatedImg.Description = img.Description;
            updatedImg.Nom = img.Nom;
            updatedImg.FileName = img.FileName;

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

        public GalerieFullViewModel GetPaginatedGallery(int idGallery, int pageSize, int PageNB)
        {
            var galerie = _context.Galeries.Where(g => g.GalerieId == idGallery && g.IsDeleted == false)
                                           .Include("ImageItems")
                                           .FirstOrDefault();
            
            if (galerie == null)
            {
                galerie = GetFirstGalerie();
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
                nbImageItems = nbImages,
                TotalPages = (int)Math.Ceiling(decimal.Divide(nbImages, pageSize)),
                ListeImages = GetPaginatedImagesItem(galerie.GalerieId, pageSize, PageNB)          
            };
        }
    }
}
