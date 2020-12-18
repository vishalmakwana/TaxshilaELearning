using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.ServiceBus.OfflineSync.Models.ThinViewModels
{
    public class StockInOutThinViewModel : BaseThinViewModel
    {
        private int _stockInOutCategorysId;
        [ModelProperty(nameof(StockInOutManagementModel.StockInOutCategorysId))]
        public int StockInOutCategorysId
        {
            get => _stockInOutCategorysId;
            set { SetProperty(ref _stockInOutCategorysId, value); }
        }

        private CategoryModel _stockInOutCategory;
        [ModelProperty(nameof(StockInOutManagementModel.StockInOutCategoryModel))]
        public CategoryModel StockInOutCategory
        {
            get => _stockInOutCategory;
            set { SetProperty(ref _stockInOutCategory, value); }
        }

        private int _stockInOutProductId;
        [ModelProperty(nameof(StockInOutManagementModel.StockInOutProductId))]
        public int StockInOutProductId
        {
            get => _stockInOutProductId;
            set { SetProperty(ref _stockInOutProductId, value); }
        }

        private ProductModel _stockInOutProduct;
        [ModelProperty(nameof(StockInOutManagementModel.StockInOutProductModel))]
        public ProductModel StockInOutProductModel
        {
            get => _stockInOutProduct;
            set { SetProperty(ref _stockInOutProduct, value); }
        }
        
        private bool _isSaved;
        [ModelProperty(nameof(StockInOutManagementModel.IsSaved))]
        public bool IsSaved
        {
            get => _isSaved;
            set { SetProperty(ref _isSaved, value); }
        }


    }
}
