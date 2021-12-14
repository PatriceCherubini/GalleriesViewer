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
using Microsoft.Extensions.Configuration;
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
        private readonly IGalerieService _galerieService;
        private readonly IConfiguration _configuration;
        [BindProperty]
        public int PageSize { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageNB { get; set; } = 1;
        public int? Show { get; private set; }
        [BindProperty(SupportsGet = true)]
        public int id { get; set; }
        public string ErrorMessage { get; set; }
        public GalerieFullViewModel Galerie { get; set; }
        [BindProperty]
        public SortType SortingList { get; set; }
        public IndexModel(IGalerieService galerieService, IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _galerieService = galerieService;
            _hostEnvironment = hostEnvironment;
            _configuration = configuration;
            PageSize = _configuration.GetValue("PageSize", 10);
        }
        public async Task<IActionResult> OnGet()
        {
            Galerie = await _galerieService.GetPaginatedGallery(id, PageSize, PageNB);
            Show = Galerie == null ? 0 : Galerie.Id;
            id = Galerie == null ? 0 : Galerie.Id;
            SortingList = Galerie == null ? SortType.DateUpload : Galerie.SortedBy;
            return Page();
        }

        public async Task<IActionResult> OnPostSort()
        {
            Galerie = await _galerieService.GetPaginatedGallery(id, PageSize, PageNB, SortingList);
            Show = Galerie == null ? 0 : Galerie.Id;
            id = Galerie == null ? 0 : Galerie.Id;
            return Page();
        }

        public async Task<PartialViewResult> OnGetViewCarouselPartial(int idImage)
        {
            var carousel = await _galerieService.ViewCarousel(idImage, id);
            return Partial("_ViewImageModalPartial", carousel);
        }
        public PartialViewResult OnGetGalleryModalPartial(int openedGallery)
        {
            return Partial("_GalleryModalPartial", new EditGalleryViewModel { Gallery = new GalerieViewModel(), OpenedGallery = openedGallery });
        }
        public async Task<PartialViewResult> OnGetGalleryModalPartialEdit(int idGallery, int openedGallery)
        {

            return Partial("_GalleryModalPartial", new EditGalleryViewModel { Gallery = await _galerieService.GetPaginatedGallery(idGallery, PageSize, PageNB), OpenedGallery = openedGallery });
        }
        public PartialViewResult OnGetImageModalPartial(int idGallery)
        {
            return Partial("_ImageModalPartial", new ImageViewModel { GalerieId = idGallery });
        }
        public async Task<PartialViewResult> OnGetImageModalPartialEdit(int idImage)
        {
            return Partial("_ImageEditModalPartial", await _galerieService.GetImage(idImage));
        }
        public IActionResult OnPostDeleteImg(int idImage, int idGallery)
        {
            _galerieService.DeleteImage(idImage, idGallery);
            return RedirectToPage("Index", new { id = idGallery, PageNB = 1 });
        }
        public IActionResult OnPostDeleteGallery(int idGallery)
        {
            _galerieService.DeleteGallery(idGallery);
            return RedirectToPage("Index", new { id = 0 });
        }
        public async Task<IActionResult> OnPostAddGallery(EditGalleryViewModel model)
        {
            Galerie = await _galerieService.GetPaginatedGallery(model.OpenedGallery, PageSize, PageNB);
            Show = model.OpenedGallery;

            if (ModelState.IsValid)
            {
                if (model.Gallery.Id == 0)
                {
                    _galerieService.AddNewGallery(model.Gallery);
                }
                else
                {
                    _galerieService.UpdateGallery(model.Gallery);
                }
            }
            return Partial("_GalleryModalPartial", model);
        }
        public async Task<PartialViewResult> OnPostAddImage(ImageViewModel model)
        {
            Galerie = await _galerieService.GetPaginatedGallery(model.GalerieId, PageSize, PageNB);
            Show = model.GalerieId;

            if (ModelState.IsValid)
            {
                model = _galerieService.UploadImage(_hostEnvironment.WebRootPath, "Images", model);
                _galerieService.AddImageInGalerie(model);
            }
            return new PartialViewResult
            {
                ViewName = "_ImageModalPartial",
                ViewData = new ViewDataDictionary<ImageViewModel>(ViewData, model)
            };
        }

        public async Task<PartialViewResult> OnPostEditImage(ImageWithoutFileViewModel model)
        {
            Galerie = await _galerieService.GetPaginatedGallery(model.GalerieId, PageSize, PageNB);
            Show = model.GalerieId;

            if (ModelState.IsValid)
            {
                _galerieService.UpdateImage(model);
            }
            return new PartialViewResult
            {
                ViewName = "_ImageEditModalPartial",
                ViewData = new ViewDataDictionary<ImageWithoutFileViewModel>(ViewData, model)
            };
        }
    }
}
