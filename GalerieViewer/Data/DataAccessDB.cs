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
        /// 
        /// </summary>
        /// <returns>A List of Galerie </returns>
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
                                        nbImageItems = g.ImageItems.Where(i => i.IsDeleted == false).Count()
                                    })
                                   .ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">The value of the property GalerieId</param>
        /// <returns>A Galerie with the property GalerieId == id or</returns>
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
        private Galerie GetFirstGalerie()
        {
            return _context.Galeries.Where(x => x.IsDeleted == false)
                                    .OrderBy(g => g.GalerieId)
                                    .FirstOrDefault();
        }

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

        public void DeleteImage(int id)
        {
            _context.ImageItems.Where(x => x.ImageItemId == id)
                               .FirstOrDefault()
                               .IsDeleted = true;

            _context.SaveChanges();
        }

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

        public void UpdateDateGalery(int id)
        {
            _context.Galeries.Where(g => g.GalerieId == id)
                             .FirstOrDefault()
                             .DateUpdate = DateTime.Now;

            _context.SaveChanges();
        }

        public void DeleteGallery(int id)
        {
            _context.Galeries.Where(x => x.GalerieId == id)
                               .FirstOrDefault()
                               .IsDeleted = true;

            DeleteAllImages(id);

            _context.SaveChanges();
        }

        public void DeleteAllImages(int id)
        {
            foreach (var item in _context.ImageItems.Where(x => x.GalerieId == id))
            {
                item.IsDeleted = true;
            }                               
        }

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

        public void UpdateImage(ImageViewModel img)
        {
            var updatedImg = _context.ImageItems.Where(i => i.ImageItemId == img.ImageItemId).FirstOrDefault();
            updatedImg.DateCreation = img.DateCreation;
            updatedImg.Description = img.Description;
            updatedImg.Nom = img.Nom;
            updatedImg.FileName = img.FileName;

            _context.SaveChanges();
        }

        public void UpdateGallery(GalerieViewModel galerie)
        {
            var updatedGallery = _context.Galeries.Where(i => i.GalerieId == galerie.Id).FirstOrDefault();
            updatedGallery.Description = galerie.Description;
            updatedGallery.Nom = galerie.Name;
            updatedGallery.DateUpdate = DateTime.Now;

            _context.SaveChanges();
        }
    }
}
