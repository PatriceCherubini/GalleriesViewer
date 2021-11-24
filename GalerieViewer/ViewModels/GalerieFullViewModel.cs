using GalerieViewer.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalerieViewer.ViewModels
{
    public class GalerieFullViewModel : GalerieViewModel
    {
        public List<ImageViewModel> ListeImages { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
