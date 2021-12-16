using GalerieViewer.Common;
using System.Collections.Generic;

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
