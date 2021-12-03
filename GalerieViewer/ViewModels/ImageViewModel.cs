using GalerieViewer.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace GalerieViewer.ViewModels
{
    public class ImageViewModel : ImageWithoutFileViewModel
    {
        [Required(ErrorMessage = "Please choose a valid picture file")]
        [Display(Name = "Upload a picture")]
        public IFormFile ImageFile { get; set; }
        public string FileName { get; set; }
        public string FileNameThumb { get; set; }
    }
}
