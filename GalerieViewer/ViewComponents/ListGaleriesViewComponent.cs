using GalerieViewer.Viewmodels;
using GalerieViewer.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalerieViewer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

/// <summary>
/// Displays the list of galleries in the left menu
/// </summary>
namespace GalerieViewer.ViewComponents
{

    public class ListGaleriesViewComponent : ViewComponent
    {
        private readonly IListGalerieService _listGalerieService;
        private readonly UserManager<ApplicationUser> _userManager;
        public string UserID { get; set; }

        public ListGaleriesViewComponent(IListGalerieService listGalerieService, UserManager<ApplicationUser> userManager)
        {
            _listGalerieService = listGalerieService;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(int show)
        {
            var vm = new ListGaleriesViewModel();
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            vm.ListeGaleries = await _listGalerieService.GenerateListGaleries(applicationUser.Id);
            vm.Show = show ;

            return View(vm);
        }
    }
}
