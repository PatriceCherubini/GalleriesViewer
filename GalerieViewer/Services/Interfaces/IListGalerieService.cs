using GalerieViewer.ViewModels;
using System.Collections.Generic;

namespace GalerieViewer.Services
{
    public interface IListGalerieService
   {
       ICollection<GalerieViewModel> GenerateListGaleries();
    }
}