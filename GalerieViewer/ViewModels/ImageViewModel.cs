using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

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
