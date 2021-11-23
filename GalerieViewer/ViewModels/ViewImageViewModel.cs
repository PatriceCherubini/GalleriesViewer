using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalerieViewer.ViewModels
{
    public class ViewImageViewModel
    {
        public ImageViewModel Image { get; set; }
        public int? IdNext { get; set; }
        public int? IdPrevious { get; set; }
        public string Position { get; set; }
    }
}
