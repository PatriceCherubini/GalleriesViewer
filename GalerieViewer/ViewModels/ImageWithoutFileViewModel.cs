using GalerieViewer.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GalerieViewer.ViewModels
{
    public class ImageWithoutFileViewModel
    {
        public int GalerieId { get; set; }
        public int ImageItemId { get; set; }
        [Display(Name = "Enter a name for your picture")]
        [MaxLengthAttribute(20, ErrorMessage = "Your name is too long. Maximum 20 caracters")]
        [Required(ErrorMessage = "A name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please, give a small description of your picture")]
        [MaxLengthAttribute(200, ErrorMessage = "Your description is too long. Maximum 200 caracters")]
        public string Description { get; set; }
        [Display(Name = "Enter the date this picture was taken.")]
        [Required(ErrorMessage = "Please specify a date")]
        [DateRange("1/1/1800 0:00:00 AM", ErrorMessage = "Please instert a valide date (between 01/01/1800 and today.")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateCreation { get; set; }
        public DateTime DateUpload { get; set; }
    }
}
