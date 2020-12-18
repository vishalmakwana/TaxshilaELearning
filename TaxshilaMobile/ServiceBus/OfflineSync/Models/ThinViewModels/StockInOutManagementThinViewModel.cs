using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.ServiceBus.OfflineSync.Models.ThinViewModels
{
    public class StockInOutManagementThinViewModel : BaseThinViewModel
    {
        #region Properties

        private int _stockInOutCategorysId;
        [ModelProperty(nameof(StockInOutManagementModel.StockInOutCategorysId))]
        public int StockInOutCategorysId
        {
            get => _stockInOutCategorysId;
            set { SetProperty(ref _stockInOutCategorysId, value); }
        }

        private CategoryModel _stockInOutCategoryModel = new CategoryModel();
        [ModelProperty(nameof(StockInOutManagementModel.StockInOutCategoryModel))]
        public CategoryModel StockInOutCategoryModel
        {
            get => _stockInOutCategoryModel;
            set { SetProperty(ref _stockInOutCategoryModel, value); }
        }

        private int _stockInOutProductId;
        [ModelProperty(nameof(StockInOutManagementModel.StockInOutProductId))]
        public int StockInOutProductId
        {
            get => _stockInOutProductId;
            set { SetProperty(ref _stockInOutProductId, value); }
        }

        private int _stockInOutOption;
        [ModelProperty(nameof(StockInOutManagementModel.StockInOutOption))]
        public int StockInOutOption
        {
            get => _stockInOutOption;
            set { SetProperty(ref _stockInOutOption, value); }
        }


        private ProductModel _stockInOutProductModel = new ProductModel();
        [ModelProperty(nameof(StockInOutManagementModel.StockInOutProductModel))]
        public ProductModel StockInOutProductModel
        {
            get => _stockInOutProductModel;
            set { SetProperty(ref _stockInOutProductModel, value); }
        }

        private bool _isSaved;
        [ModelProperty(nameof(StockInOutManagementModel.IsSaved))]
        public bool IsSaved
        {
            get => _isSaved;
            set { SetProperty(ref _isSaved, value); }
        }

        
        private List<StockInOutEntryModel> _stockInOutEntrysCollections = new List<StockInOutEntryModel>();
        [ModelProperty(nameof(StockInOutManagementModel.StockInOutEntrys))]
        public List<StockInOutEntryModel> StockInOutEntrysCollections
        {
            get => _stockInOutEntrysCollections;
            set { SetProperty(ref _stockInOutEntrysCollections, value); }
        }
        #endregion
        
        #region Derived Properties
        public string InOutOption
        {
            get
            {

                if (StockInOutOption == 1)
                {
                    return "Stock In";
                }
                else
                {
                    return "Stock Out";
                }
            }
        }
        #endregion

    }
}
