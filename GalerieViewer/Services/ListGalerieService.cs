using GalerieViewer.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GalerieViewer.Services
{
    public class ListGalerieService : IListGalerieService
    {
        readonly IDataAccess _dataAccess;
        public ListGalerieService(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }
        /// <summary>
        /// Get all galleries
        /// </summary>
        /// <returns>A collection of all galleries (in GalerieViewModel format)</returns>
        public async Task<ICollection<GalerieViewModel>> GenerateListGaleries(string userID)
        {
            return await _dataAccess.GetAllGaleries(userID);
        }
    }
}
