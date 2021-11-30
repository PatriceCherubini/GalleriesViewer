using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalerieViewer.Common
{
    public enum SortType

    {
        [Display(Name = "Date of Creation")]
        DateCreation,
        [Display(Name = "Last Updated")]
       DateUpload,
        Name
    }
}
