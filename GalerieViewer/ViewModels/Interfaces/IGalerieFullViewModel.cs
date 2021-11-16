using GalerieViewer.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace GalerieViewer.ViewModels
{
    public interface IGalerieFullViewModel
    {
        List<ImageViewModel> ListeImages { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}