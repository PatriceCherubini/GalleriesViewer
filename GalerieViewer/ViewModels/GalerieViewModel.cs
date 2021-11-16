using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GalerieViewer.ViewModels
{
    public class GalerieViewModel : IGalerieViewModel
    {
        public int Id { get; set; }
        public int? nbImageItems { get; set; }
        [Display(Name = "Enter a name for your gallery")]
        [MaxLengthAttribute(20, ErrorMessage = "Your name is too long. Maximum 20 caracters")]
        [Required(ErrorMessage = "A name is required.")]
        public string Name { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateUpdate { get; set; }
        [Required(ErrorMessage = "Please, give a small description of your gallery")]
        [MaxLengthAttribute(200, ErrorMessage = "Your description is too long. Maximum 200 caracters")]
        public string Description { get; set; }

    }
}
