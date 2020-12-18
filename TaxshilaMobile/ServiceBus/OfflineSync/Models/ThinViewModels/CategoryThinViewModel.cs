using TaxshilaMobile.DataTypesApp.Default;
using MvvmHelpers;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.ServiceBus.OfflineSync.Models.ThinViewModels
{
    public class CategoryThinViewModel: BaseThinViewModel
    {

        private string _name;
        [ModelProperty(nameof(CategoryModel.Name))]
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);                
            }
        }

        private string _description;
        [ModelProperty(nameof(CategoryModel.Description))]
        public string Description
        {
            get => _description;
            set
            {
                SetProperty(ref _description, value);
            }
        }

        private string _imageUrl;
        [ModelProperty(nameof(CategoryModel.ImageUrl))]
        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                SetProperty(ref _imageUrl, value);
            }
        }


        private bool _isfeatured;
        [ModelProperty(nameof(CategoryModel.IsFeatured))]
        public bool IsFeatured
        {
            get => _isfeatured;
            set
            {
                SetProperty(ref _isfeatured, value);
            }
        }


        private bool _isSaved;
        [ModelProperty(nameof(CategoryModel.IsSaved))]
        public bool IsSaved
        {
            get => _isSaved;
            set
            {
                SetProperty(ref _isSaved, value);
            }
        }

        private ObservableRangeCollection<ProductThinViewModel> _productCollection = new ObservableRangeCollection<ProductThinViewModel>();

        [ModelProperty(nameof(CategoryModel.Products))]
        public ObservableRangeCollection<ProductThinViewModel> ProductCollection
        {
            get => _productCollection;
            set
            {
                SetProperty(ref _productCollection, value);
            }
        }


        private ObservableRangeCollection<StockInOutManagementThinViewModel> _stockInOutManagement = new ObservableRangeCollection<StockInOutManagementThinViewModel>();

        [ModelProperty(nameof(CategoryModel.StockInOutManagements))]
        public ObservableRangeCollection<StockInOutManagementThinViewModel> StockInOutManagements
        {
            get => _stockInOutManagement;
            set
            {
                SetProperty(ref _stockInOutManagement, value);
            }
        }


    }
}
