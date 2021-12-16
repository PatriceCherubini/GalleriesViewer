using GalerieViewer.ViewModels;
using System.Collections.Generic;

namespace GalerieViewer.Viewmodels
{
    public class ListGaleriesViewModel 
    {
        public ICollection<GalerieViewModel> ListeGaleries { get; set; }
        public int Show { get; set; }
    }
}
