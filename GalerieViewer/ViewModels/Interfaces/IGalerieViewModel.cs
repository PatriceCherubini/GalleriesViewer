using System;

namespace GalerieViewer.ViewModels
{
    public interface IGalerieViewModel
    {
        DateTime DateCreation { get; set; }
        DateTime DateUpdate { get; set; }
        string Description { get; set; }
        int Id { get; set; }
        string Name { get; set; }
        int? nbImageItems { get; set; }
    }
}