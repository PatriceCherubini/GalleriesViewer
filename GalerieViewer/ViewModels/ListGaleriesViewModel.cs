using GalerieViewer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalerieViewer.Viewmodels
{
    public class ListGaleriesViewModel 
    {
        public ICollection<GalerieViewModel> ListeGaleries { get; set; }
        public int Show { get; set; }
    }
}
