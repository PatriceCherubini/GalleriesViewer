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
using System.IO;
using System.Linq;
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
        
        public int? Show { get; private set; }
        public string ErrorMessage { get; set; }
        public GalerieFullViewModel Galerie { get; set; }
        public IndexModel(ILogger<IndexModel> logger, IGalerieService galerieService, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _galerieService = galerieService;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult OnGet(int id)
        {
           
            Galerie = _galerieService.GenerateGalerie(id);
            Show = Galerie == null ? 0 : Galerie.Id;
            return Page();
        }

        public PartialViewResult OnGetGalleryModalPartial(int openedGallery)
        {
            return Partial("_GalleryModalPartial", new EditGalleryViewModel { Gallery = new GalerieViewModel(), OpenedGallery = openedGallery });
        }
        public PartialViewResult OnGetGalleryModalPartialEdit(int idGallery, int openedGallery)
        {
            return Partial("_GalleryModalPartial", new EditGalleryViewModel { Gallery = _galerieService.GenerateGalerie(idGallery), OpenedGallery = openedGallery });
        }
        public PartialViewResult OnGetImageModalPartial(int idGallery)
        {
            return Partial("_ImageModalPartial", new ImageViewModel { GalerieId = idGallery });
        }

        public PartialViewResult OnGetImageModalPartialEdit(int idImage)
        {
            return Partial("_ImageModalPartial", _galerieService.GetImage(idImage));
        }

        //public IActionResult OnPostEditImg(int idImage, int openedGallery)
        //{
        //    Galerie = _galerieService.GenerateGalerie(openedGallery);
        //    AddedImage = _galerieService.GetImage(idImage);
        //    Show = openedGallery;
        //    return Page();
        //}
        //public IActionResult OnPostEditGallery(int IdGallery)
        //{
        //    Galerie = _galerieService.GenerateGalerie(IdGallery);
        //    AddedGallery = _galerieService.GenerateGalerie(IdGallery);
        //    Show = IdGallery;
        //    return Page();
        //}
        public IActionResult OnPostDeleteImg(int idImage, int idGallery)
        {
            _galerieService.DeleteImage(idImage, idGallery);
            return RedirectToPage("Index", new { id = idGallery });
        }
        public IActionResult OnPostDeleteGallery(int idGallery)
        {
            _galerieService.DeleteGallery(idGallery);
            return RedirectToPage("Index", new { id = 0 });
        }
        public IActionResult OnPostAddGallery(EditGalleryViewModel model)
        {
            Galerie = _galerieService.GenerateGalerie(model.OpenedGallery);
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
            Galerie = _galerieService.GenerateGalerie(model.GalerieId);
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

            //return Partial("_ImageyModalPartial", new ImageViewModel());
        }

        //public IActionResult OnPostAddImage(int openedGallery)
        //{
        //    Galerie = _galerieService.GenerateGalerie(openedGallery);
        //    Show = openedGallery;

        //    if ((ModelState["AddedImage.Nom"].ValidationState == ModelValidationState.Valid)
        //        && (ModelState["AddedImage.Description"].ValidationState == ModelValidationState.Valid)
        //        && (ModelState["AddedImage.DateCreation"].ValidationState == ModelValidationState.Valid)
        //        && (ModelState["AddedImage.ImageFile"].ValidationState == ModelValidationState.Valid))
        //    {
        //        AddedImage.FileName = _galerieService.UploadImage(_hostEnvironment.WebRootPath, "Images", AddedImage);

        //        if (AddedImage.ImageItemId == 0)
        //        {
        //            _galerieService.AddImageInGalerie(AddedImage);
        //        }
        //        else
        //        {
        //            _galerieService.UpdateImage(AddedImage);
        //        }
        //        return RedirectToPage("Index", new { id = openedGallery });
        //    }
        //    return Page();
        //}
    }
}
