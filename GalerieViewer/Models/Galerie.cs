using GalerieViewer.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalerieViewer.Models
{
    public class Galerie 
    {
        public int GalerieId { get; set; }
        public int UserId { get; set; }
        public bool IsDeleted { get; set; } = false;
        [Required]
        [Column(TypeName = "nvarchar(25)")]
        public string Nom { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        [Required]
        public DateTime DateCreation { get; set; }
        [Required]
        public DateTime DateUpdate { get; set; } 
        public List<ImageItem> ImageItems { get; set; }
        public SortType SortedBy { get; set; }
        public string ImageIcon { get; set; }

    }
}
