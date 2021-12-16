using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GalerieViewer.Models
{ 
    public class ImageItem 
    {
        public int ImageItemId { get; set; }
        public int GalerieId { get; set; }
        public bool IsDeleted { get; set; } = false;
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string Name { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        [Required]
        public DateTime? DateCreation { get; set; }
        [Required]
        public DateTime? DateUpload { get; set; }
        public string FileName { get; set; }
        public string FileNameThumb { get; set; }
    }
}
