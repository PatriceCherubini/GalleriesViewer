using GalerieViewer.Data;
using GalerieViewer.Models;
using GalerieViewer.Services;
using GalerieViewer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        private readonly IPictureUploader _pictureUploader;
        public int? Show { get; private set; }
        public string ErrorMessage { get; set; }
        public GalerieFullViewModel Galerie { get; set; }
        [BindProperty]
        public ImageViewModel AddedImage { get; set; }
        [BindProperty]
        public GalerieViewModel AddedGallery { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IGalerieService galerieService, IWebHostEnvironment hostEnvironment, IPictureUploader pictureUploader)
        {
            _logger = logger;
            _galerieService = galerieService;
            _hostEnvironment = hostEnvironment;
            _pictureUploader = pictureUploader;
        }

        public IActionResult OnGet(int id, int? show)
        {
           
            Galerie = _galerieService.GenerateGalerie(id);
            Show = Galerie == null ? 0 : Galerie.Id;
            return Page();
        }

        public IActionResult OnPostEditImg(int idImage, int openedGallery)
        {
            Galerie = _galerieService.GenerateGalerie(openedGallery);
            AddedImage = _galerieService.GetImage(idImage);
            Show = openedGallery;
            return Page();
        }
        public IActionResult OnPostEditGallery(int IdGallery)
        {
            Galerie = _galerieService.GenerateGalerie(IdGallery);
            AddedGallery = _galerieService.GenerateGalerie(IdGallery);
            Show = IdGallery;
            return Page();
        }
        public IActionResult OnPostDeleteImg(int idImage, int idGallery)
        {
            _galerieService.DeleteImage(idImage, idGallery);
            return RedirectToPage("Index", new { id = idGallery, show = idGallery });
        }
        public IActionResult OnPostDeleteGallery(int idGallery)
        {
            _galerieService.DeleteGallery(idGallery);
            return RedirectToPage("Index", new { id = 0, show = idGallery });
        }
        public IActionResult OnPostAddImage(int openedGallery)
        {
            Galerie = _galerieService.GenerateGalerie(openedGallery);
            Show = openedGallery;
            
            if ((ModelState["AddedImage.Nom"].ValidationState == ModelValidationState.Valid)
                && (ModelState["AddedImage.Description"].ValidationState == ModelValidationState.Valid)
                && (ModelState["AddedImage.DateCreation"].ValidationState == ModelValidationState.Valid)
                && (ModelState["AddedImage.ImageFile"].ValidationState == ModelValidationState.Valid))
            {
                string rootPath = _hostEnvironment.WebRootPath;
                string uniqueFileName = _pictureUploader.SetFileName(AddedImage.ImageFile, AddedImage.Nom);
                _pictureUploader.SetPath(rootPath, "Images");
                _pictureUploader.Upload();
                AddedImage.FileName = uniqueFileName;

                if (AddedImage.ImageItemId == 0)
                {
                    _galerieService.AddImageInGalerie(AddedImage);
                }
                else
                {
                    _galerieService.UpdateImage(AddedImage);
                }
                return RedirectToPage("Index", new { id = openedGallery, show = openedGallery });
            }
            return Page();
        }
        public IActionResult OnPostAddGallery(int openedGallery)
        {
            Galerie = _galerieService.GenerateGalerie(openedGallery);
            Show = openedGallery;
            int toOpenGallery = 0;

            if ((ModelState["AddedGallery.Name"].ValidationState == ModelValidationState.Valid)
                && (ModelState["AddedGallery.Description"].ValidationState == ModelValidationState.Valid))
            {
                if (AddedGallery.Id == 0)
                {
                    toOpenGallery = _galerieService.AddNewGallery(AddedGallery);
                }
                else
                {
                    _galerieService.UpdateGallery(AddedGallery);
                    toOpenGallery = AddedGallery.Id;
                }

                return RedirectToPage("Index", new { id = toOpenGallery, show = toOpenGallery });
            }
            return Page();
        }
    }
}
