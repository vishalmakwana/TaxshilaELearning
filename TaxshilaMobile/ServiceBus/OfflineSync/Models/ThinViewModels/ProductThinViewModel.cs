using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.ServiceBus.OfflineSync.Models.ThinViewModels
{
    public class ProductThinViewModel: BaseThinViewModel
    {
        private string _name;
        [ModelProperty(nameof(ProductModel.Name))]
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
            }
        }

        private bool _isSaved;
        [ModelProperty(nameof(ProductModel.IsSaved))]
        public bool IsSaved
        {
            get => _isSaved;
            set
            {
                SetProperty(ref _isSaved, value);
            }
        }

        private string _description;
        [ModelProperty(nameof(ProductModel.Description))]
        public string Description
        {
            get => _description;
            set
            {
                SetProperty(ref _description, value);
            }
        }

        private string _imageUrl;
        [ModelProperty(nameof(ProductModel.ImageUrl))]
        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                SetProperty(ref _imageUrl, value);
            }
        }

        private bool _status;
        [ModelProperty(nameof(ProductModel.Status))]
        public bool Status
        {
            get => _status;
            set
            {
                SetProperty(ref _status, value);
            }
        }

        private string _itemCode;
        [ModelProperty(nameof(ProductModel.ItemCode))]
        public string ItemCode
        {
            get => _itemCode;
            set
            {
                SetProperty(ref _itemCode, value);
            }
        }

        private int _categorysId;
        [ModelProperty(nameof(ProductModel.CategorysId))]
        public int CategorysId
        {
            get => _categorysId;
            set
            {
                SetProperty(ref _categorysId, value);
            }
        }

        

        private double _costPrice;
        [ModelProperty(nameof(ProductModel.CostPrice))]
        public double CostPrice
        {
            get => _costPrice;
            set
            {
                SetProperty(ref _costPrice, value);
            }
        }

        private double _salePrice;
        [ModelProperty(nameof(ProductModel.SalePrice))]
        public double SalePrice
        {
            get => _salePrice;
            set
            {
                SetProperty(ref _salePrice, value);
            }
        }

        private CategoryModel _productCategoryModel = new CategoryModel();
        [ModelProperty(nameof(ProductModel.ProductCategory))]
        public CategoryModel ProductCategoryModel
        {
            get => _productCategoryModel;
            set
            {
                SetProperty(ref _productCategoryModel, value);
            }
        }


        private MeasurementTypeModel _productMeasurementType = new MeasurementTypeModel();
        [ModelProperty(nameof(ProductModel.ProductMeasurementType))]
        public MeasurementTypeModel ProductMeasurementType
        {
            get => _productMeasurementType;
            set
            {
                SetProperty(ref _productMeasurementType, value);
            }  
        }

        private int _measurementTypeId;
        [ModelProperty(nameof(ProductModel.MeasurementTypeId))]
        public int MeasurementTypeId
        {
            get => _measurementTypeId;
            set
            {
                SetProperty(ref _measurementTypeId, value);
            }
        }


    }
}
