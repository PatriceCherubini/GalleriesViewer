using GalerieViewer.Viewmodels;
using GalerieViewer.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GalerieViewer.ViewComponents
{
    public class ListGaleriesViewComponent : ViewComponent
    {
        private IListGalerieService _listGalerieService;

        public ListGaleriesViewComponent(IListGalerieService listGalerieService)
        {
            _listGalerieService = listGalerieService;
        }
        public async Task<IViewComponentResult> InvokeAsync(int show)
        {
            var vm = new ListGaleriesViewModel();
            vm.ListeGaleries = await _listGalerieService.GenerateListGaleries();
            vm.Show = show ;

            return View(vm);
        }
    }
}
