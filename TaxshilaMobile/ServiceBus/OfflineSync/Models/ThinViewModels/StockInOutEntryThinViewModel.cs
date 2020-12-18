using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.ServiceBus.OfflineSync.Models.ThinViewModels
{
    public class StockInOutEntryThinViewModel : BaseThinViewModel
    {

        private int _stockInOutManagementId;
        [ModelProperty(nameof(StockInOutEntryModel.StockInOutManagementId))]
        public int StockInOutManagementId
        {
            get => _stockInOutManagementId;
            set { SetProperty(ref _stockInOutManagementId, value); }
        }

        private StockInOutManagementThinViewModel _stockInOutManagementThinViewModel;
        [ModelProperty(nameof(StockInOutEntryModel.StockInOutManagement))]
        public StockInOutManagementThinViewModel StockInOutManagementThinViewModel
        {
            get => _stockInOutManagementThinViewModel;
            set { SetProperty(ref _stockInOutManagementThinViewModel, value); }
        }


        private int _stockInOutUnitId;
        [ModelProperty(nameof(StockInOutEntryModel.StockInOutUnitId))]
        public int StockInOutUnitId
        {
            get => _stockInOutUnitId;
            set { SetProperty(ref _stockInOutUnitId, value); }
        }

        private UnitsThinViewModel stockInOutUnitThinViewModel;
        [ModelProperty(nameof(StockInOutEntryModel.StockInOutUnit))]
        public UnitsThinViewModel StockInOutUnitThinViewModel
        {
            get => stockInOutUnitThinViewModel;
            set { SetProperty(ref stockInOutUnitThinViewModel, value); }
        }

        private int _stockCount;
        [ModelProperty(nameof(StockInOutEntryModel.StockCount))]
        public int StockCount
        {
            get => _stockCount;
            set { SetProperty(ref _stockCount, value); }
        }

        private int _stockInOutOption;
        [ModelProperty(nameof(StockInOutEntryModel.StockInOutOption))]
        public int StockInOutOption
        {
            get => _stockInOutOption;
            set { SetProperty(ref _stockInOutOption, value); }
        }
        private string _unitTitle;
        [ModelProperty(nameof(StockInOutEntryModel.UnitTitle))]
        public string UnitTitle
        {
            get => _unitTitle;
            set { SetProperty(ref _unitTitle, value); }
        }

        private int _stockInOutProductId;
        [ModelProperty(nameof(StockInOutEntryModel.StockInOutProductId))]
        public int StockInOutProductId
        {
            get => _stockInOutProductId;
            set { SetProperty(ref _stockInOutProductId, value); }
        }

        private ProductModel _stockInOutProduct;
        [ModelProperty(nameof(StockInOutEntryModel.StockInOutProduct))]
        public ProductModel StockInOutProduct
        {
            get => _stockInOutProduct;
            set { SetProperty(ref _stockInOutProduct, value); }
        }


    }
}
