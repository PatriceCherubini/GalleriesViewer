using GalerieViewer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalerieViewer.ViewModels
{
    public class CarouselViewModel 
    {
        public List<ImageViewModel> ListPictures { get; set; }
        public int Position { get; set; }
        public SortType SortedBy { get; set; }
        public int NbPictures { get { return ListPictures.Count; } }
    }
}
