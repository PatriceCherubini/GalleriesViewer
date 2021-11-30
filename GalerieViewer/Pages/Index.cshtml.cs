using GalerieViewer.Common;
using GalerieViewer.Data;
using GalerieViewer.Models;
using GalerieViewer.Services;
using GalerieViewer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GalerieViewer.Pages
{
    //[Authorize]
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<IndexModel> _logger;
        private IGalerieService _galerieService;
        [BindProperty]
        public int PageSize { get; set; } = 5;
        [BindProperty(SupportsGet = true)]
        public int PageNB { get; set; } = 1;
        public int? Show { get; private set; }
        [BindProperty(SupportsGet = true)]
        public int id { get; set; }
        public string ErrorMessage { get; set; }
        public GalerieFullViewModel Galerie { get; set; }
        [BindProperty]
        public SortType SortingList { get; set; } 
        public IndexModel(ILogger<IndexModel> logger, IGalerieService galerieService, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _galerieService = galerieService;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult OnGet()
        {  
            Galerie = _galerieService.GetPaginatedGallery(id, PageSize, PageNB);
            Show = Galerie == null ? 0 : Galerie.Id;
            id = Galerie == null ? 0 : Galerie.Id;
            return Page();
        }

        public IActionResult OnPostSort()
        {
            var sorted = SortingList.ToString();
            Galerie = _galerieService.GetPaginatedGallery(id, PageSize, PageNB, sorted);
            Show = Galerie == null ? 0 : Galerie.Id;
            id = Galerie == null ? 0 : Galerie.Id;
            return Page();
        }


        public PartialViewResult OnGetViewImageModalPartial(int idImage)
        {
            return new PartialViewResult
            {
                ViewName = "_ViewImageCarousel",
                ViewData = new ViewDataDictionary<ViewImageViewModel>(ViewData, _galerieService.ViewImage(idImage, id))
            };
           // return Partial("_ViewImageCarousel", _galerieService.ViewImage(idImage, id));
        }
        public PartialViewResult OnGetViewCarouselPartial(int idImage)
        {
            return Partial("_ViewImageModalPartial", _galerieService.ViewCarousel(idImage, id));
        }
        public PartialViewResult OnGetGalleryModalPartial(int openedGallery)
        {
            return Partial("_GalleryModalPartial", new EditGalleryViewModel { Gallery = new GalerieViewModel(), OpenedGallery = openedGallery });
        }
        public PartialViewResult OnGetGalleryModalPartialEdit(int idGallery, int openedGallery)
        {
            return Partial("_GalleryModalPartial", new EditGalleryViewModel { Gallery = _galerieService.GetGallery(idGallery), OpenedGallery = openedGallery });
        }
        public PartialViewResult OnGetImageModalPartial(int idGallery)
        {
            return Partial("_ImageModalPartial", new ImageViewModel { GalerieId = idGallery });
        }
        public PartialViewResult OnGetImageModalPartialEdit(int idImage)
        {
            return Partial("_ImageModalPartial", _galerieService.GetImage(idImage));
        }
        public IActionResult OnPostDeleteImg(int idImage, int idGallery)
        {
            _galerieService.DeleteImage(idImage, idGallery);
            return RedirectToPage("Index", new { id = idGallery, PageNB = 1});
        }
        public IActionResult OnPostDeleteGallery(int idGallery)
        {
            _galerieService.DeleteGallery(idGallery);
            return RedirectToPage("Index", new { id = 0 });
        }
        public IActionResult OnPostAddGallery(EditGalleryViewModel model)
        {
            Galerie = _galerieService.GetPaginatedGallery(model.OpenedGallery, PageSize, PageNB);
            Show = model.OpenedGallery;
            int toOpenGallery = 0;

            if (ModelState.IsValid)
            {
                if (model.Gallery.Id == 0)
                {
                    toOpenGallery = _galerieService.AddNewGallery(model.Gallery);
                }
                else
                {
                    _galerieService.UpdateGallery(model.Gallery);
                    toOpenGallery = model.Gallery.Id;
                }
            }
            return Partial("_GalleryModalPartial", model);
        }
        public PartialViewResult OnPostAddImage(ImageViewModel model)
        {
            Galerie = _galerieService.GetPaginatedGallery(model.GalerieId, PageSize, PageNB);
            Show = model.GalerieId;

            if (ModelState.IsValid)
            {
                model.FileName = _galerieService.UploadImage(_hostEnvironment.WebRootPath, "Images", model);

                if (model.ImageItemId == 0)
                {
                    _galerieService.AddImageInGalerie(model);
                }
                else
                {
                    _galerieService.UpdateImage(model);
                }
            }
            return new PartialViewResult
            {
                ViewName = "_ImageModalPartial",
                ViewData = new ViewDataDictionary<ImageViewModel>(ViewData, model)
            };
        }
    }
}
