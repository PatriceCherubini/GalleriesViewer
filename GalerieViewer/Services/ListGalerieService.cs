using GalerieViewer.Data;
using GalerieViewer.Models;
using GalerieViewer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public ICollection<GalerieViewModel> GenerateListGaleries()
        {
            return _dataAccess.GetAllGaleries();
        }
    }
}
