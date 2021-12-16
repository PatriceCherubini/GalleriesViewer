using GalerieViewer.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GalerieViewer.Services
{
    public interface IListGalerieService
   {
       Task <ICollection<GalerieViewModel>> GenerateListGaleries(string userID);
    }
}