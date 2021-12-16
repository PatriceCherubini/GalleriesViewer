using GalerieViewer.Common;
using GalerieViewer.Data;
using GalerieViewer.Services;
using GalerieViewer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GalerieViewer.Pages
{
    [Authorize]
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IGalerieService _galerieService;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
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

        public IndexModel(IGalerieService galerieService, IWebHostEnvironment hostEnvironment, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _galerieService = galerieService;
            _hostEnvironment = hostEnvironment;
            _configuration = configuration;
            _userManager = userManager;

            PageSize = _configuration.GetValue("PageSize", 10);
        }
        /// <summary>
        /// Main Get. Calls _galeryservice which return a paginated Gallery (GalleryFullViewModel) and asigns it to the Gallery property. Also asigns Show, id and SortingList
        /// </summary>
        /// <returns>The Index.cshtml page</returns>
        public async Task<IActionResult> OnGet()
        {
            if (id == 1)
            {
                string userId = (await _userManager.GetUserAsync(User)).Id;
                Galerie = await _galerieService.GetDefautPaginatedGallery(userId, PageSize, PageNB);
            }
            else
            {
                Galerie = await _galerieService.GetPaginatedGallery(id, PageSize, PageNB);
            }
            
            Show = Galerie == null ? 0 : Galerie.Id;
            id = Galerie == null ? 0 : Galerie.Id;
            SortingList = Galerie == null ? SortType.DateUpload : Galerie.SortedBy;
            return Page();
        }
        /// <summary>
        /// This method is called when the sort button is clicked. Return a gallery sorted by the value of SortingList.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostSort()
        {
            Galerie = await _galerieService.GetPaginatedGallery(id, PageSize, PageNB, SortingList);
            Show = Galerie == null ? 0 : Galerie.Id;
            id = Galerie == null ? 0 : Galerie.Id;
            return Page();
        }
        /// <summary>
        /// This method is called when the user clicks on a picture. 
        /// </summary>
        /// <param name="idImage">Id of the clicked picture. Use to display the specific picture in the carousel</param>
        /// <returns>A partial view which contain a modal that display the picture gallery in a carousel</returns>
        public async Task<PartialViewResult> OnGetViewCarouselPartial(int idImage)
        {
            var carousel = await _galerieService.ViewCarousel(idImage, id);
            return Partial("_ViewImageModalPartial", carousel);
        }
        /// <summary>
        /// This method is called when the user clicks on a "add gallery" button.
        /// </summary>
        /// <param name="openedGallery">The id of the gallery this is currently opened</param>
        /// <returns>A partial view which contain a form to add a new gallery</returns>
        public async Task<PartialViewResult> OnGetGalleryModalPartial(int openedGallery)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            return Partial("_GalleryModalPartial", new EditGalleryViewModel { Gallery = new GalerieViewModel() { UserId = (await _userManager.GetUserAsync(User)).Id }, OpenedGallery = openedGallery });
        }
        /// <summary>
        /// This method is called when the user clicks on a "edit gallery" button.
        /// </summary>
        /// <param name="idGallery">The id of the gallery to edit</param>
        /// <param name="openedGallery">The id of the opened gallery</param>
        /// <returns>A partial view which contain a form to edit the specific gallery.</returns>
        public async Task<PartialViewResult> OnGetGalleryModalPartialEdit(int idGallery, int openedGallery)
        {
            return Partial("_GalleryModalPartial", new EditGalleryViewModel { Gallery = await _galerieService.GetPaginatedGallery(idGallery, PageSize, PageNB), OpenedGallery = openedGallery });
        }
        /// <summary>
        /// This method is called when the user clicks on a "add picture" button.
        /// </summary>
        /// <param name="idGallery">The id of the gallery which will contain the new picture</param>
        /// <returns>A partial view which contain a form to add a new picture. Contains the upload field</returns>
        public PartialViewResult OnGetImageModalPartial(int idGallery)
        {
            return Partial("_ImageModalPartial", new ImageViewModel { GalerieId = idGallery });
        }
        /// <summary>
        /// This method is called when the user clicks on a "edit picture" button.
        /// </summary>
        /// <param name="idImage">The id of the picture to edit</param>
        /// <returns>A partial view which contain a form to edit the picture. Doesn't contain the upload field</returns>
        public async Task<PartialViewResult> OnGetImageModalPartialEdit(int idImage)
        {
            return Partial("_ImageEditModalPartial", await _galerieService.GetImage(idImage));
        }
        /// <summary>
        /// This method is called when the user click on "delete picture" button.
        /// </summary>
        /// <param name="idImage">Id of the picture to delete</param>
        /// <param name="idGallery">Id of the gallery which contains the picture</param>
        /// <returns>Redirect to the index after deleting the picture</returns>
        public IActionResult OnPostDeleteImg(int idImage, int idGallery)
        {
            _galerieService.DeleteImage(idImage, idGallery);
            return RedirectToPage("Index", new { id = idGallery, PageNB = 1 });
        }
        /// <summary>
        /// This method is called when the user click on "delete gallery" button-
        /// </summary>
        /// <param name="idGallery">Id of the gallery to delete</param>
        /// <returns>Redirect to the index after deleting the gallery</returns>
        public IActionResult OnPostDeleteGallery(int idGallery)
        {
            _galerieService.DeleteGallery(idGallery);
            return RedirectToPage("Index", new { id = 0 });
        }
        /// <summary>
        /// This method is called when a form to add/edit a gallery is sent via a post. Datas are verified and if valids, the gallery is either updated or added. 
        /// </summary>
        /// <param name="model">Data sent by the user</param>
        /// <returns>Return the modal with the form and with eventual erros. A javascript will take care of the rest</returns>
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
        /// <summary>
        /// This method is called when a form to add new picture is sent. Datas are verified and id valid, a new picture is added.
        /// </summary>
        /// <param name="model">Data sent by the user</param>
        /// <returns>Return the modal with the form and with eventual erros. A javascript will take care of the rest</returns>
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
        /// <summary>
        /// This method is called when a form to edit a picture is sent. Datas are verified and if valids, the pictures' attributes are updated.
        /// </summary>
        /// <param name="model">Data sent by the user</param>
        /// <returns>Return the modal with the form and with eventual erros. A javascript will take care of the rest</returns>
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
