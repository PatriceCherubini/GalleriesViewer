using GalerieViewer.ViewModels;
using System.Collections.Generic;

namespace GalerieViewer.Viewmodels
{
    public interface IListGaleriesViewModel
    {
       ICollection<GalerieViewModel> ListeGaleries { get; set; }
       int Show { get; set; }
    }
}