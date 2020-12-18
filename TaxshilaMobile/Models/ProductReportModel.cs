using TaxshilaMobile.ServiceBus.OfflineSync;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using TaxshilaMobile.ServiceBus.OfflineSync.Models.ThinViewModels;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.Models
{
    public class ProductReportModel : BaseViewModel
    {
        private ProductModel _selectedtProduct;

        public ProductModel SelectedtProduct
        {
            get => _selectedtProduct;
            set { SetProperty(ref _selectedtProduct, value); }
        }

        private List<UnitModel> _unitsList;

        public List<UnitModel> UnitsList
        {
            get => _unitsList;
            set { SetProperty(ref _unitsList, value); }
        }


    }


}
