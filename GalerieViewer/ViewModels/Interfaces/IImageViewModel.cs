using System;

namespace GalerieViewer.ViewModels
{
    public interface IImageViewModel
    {
        DateTime DateCreation { get; set; }
        string Description { get; set; }
        int GalerieId { get; set; }
        int ImageItemId { get; set; }
        string Nom { get; set; }
        public string FileName { get; set; }
    }
}