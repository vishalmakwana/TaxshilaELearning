using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Models;
using TaxshilaMobile.ServiceBus.OfflineSync;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using TaxshilaMobile.Services.Interfaces;
using TaxshilaMobile.Validations;
using TaxshilaMobile.ViewModels.BaseViewModels;
using Prism.AppModel;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxshilaMobile.ViewModels.Products
{
    public class AddProductPageViewModel : BasePageViewModel<AddProductPageViewModel, IProductService>, IAutoInitialize, IViewModelExtensions
    {
        #region Services
        private IProductService _productService;
        private ICategoryService _categoryService;
        private IUnitService _unitService;
        private IMeasurementTypeService _measurementTypeService;
        #endregion

        private int _serverId;
        [ModelProperty(nameof(ModelBase.ServerId))]
        public int ServerId
        {
            get => _serverId;
            set { SetProperty(ref _serverId, value); }
        }

        private PickerItem _productCategory;
        public PickerItem ProductCategory
        {
            get => _productCategory;
            set
            {
                SetProperty(ref _productCategory, value);
            }
        }

        private PickerItem _productMeasurementTypePicker;
        public PickerItem ProductMeasurementTypePicker
        {
            get => _productMeasurementTypePicker;
            set
            {
                SetProperty(ref _productMeasurementTypePicker, value);
            }
        }

        private int _localId;
        [ModelProperty(nameof(ModelBase.LocalId))]
        public int LocalId
        {
            get => _localId;
            set { SetProperty(ref _localId, value); }
        }
        private string _modifiedBy;
        [ModelProperty(nameof(ModelBase.ModifiedBy))]
        public string ModifiedBy
        {
            get => _modifiedBy;
            set { SetProperty(ref _modifiedBy, value); }
        }
        private DateTime? _modifiedAt;
        [ModelProperty(nameof(ModelBase.ModifiedAt))]
        public DateTime? ModifiedAt
        {
            get => _modifiedAt;
            set { SetProperty(ref _modifiedAt, value); }
        }
        private string _createdBy;
        [ModelProperty(nameof(ModelBase.CreatedBy))]
        public string CreatedBy
        {
            get => _createdBy;
            set { SetProperty(ref _createdBy, value); }
        }
        private DateTime? _createdAt;
        [ModelProperty(nameof(ModelBase.CreatedAt))]
        public DateTime? CreatedAt
        {
            get => _createdAt;
            set { SetProperty(ref _createdAt, value); }
        }
        private Operation _operation = Operation.Inserted;
        [ModelProperty(nameof(ModelBase.Operation))]
        public Operation Operation
        {
            get => _operation;
            set { SetProperty(ref _operation, value); }
        }

        private bool _isActive;
        [ModelProperty(nameof(ModelBase.IsActive))]
        public bool IsActive
        {
            get => _isActive;
            set { SetProperty(ref _isActive, value); }
        }
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
        //private int _unitsId;
        //[ModelProperty(nameof(ProductModel.UnitsId))]
        //public int UnitsId
        //{
        //    get => _unitsId;
        //    set
        //    {
        //        SetProperty(ref _unitsId, value);
        //    }
        //}
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

        private ValidatableObject<PickerItem> _validateProductCategory;
        public ValidatableObject<PickerItem> ValidateProductCategory
        {
            get => _validateProductCategory;
            set
            {
                SetProperty(ref _validateProductCategory, value);
                RaisePropertyChanged(() => ValidateProductCategory);
            }
        }

        private ValidatableObject<PickerItem> _validateProductMeasurementType;
        public ValidatableObject<PickerItem> ValidateProductMeasurementType
        {
            get => _validateProductMeasurementType;
            set
            {
                SetProperty(ref _validateProductMeasurementType, value);
                RaisePropertyChanged(() => ValidateProductMeasurementType);
            }
        }



        //private ValidatableObject<PickerItem> _validateProductUnits;
        //public ValidatableObject<PickerItem> ValidateProductUnits
        //{
        //    get => _validateProductUnits;
        //    set
        //    {
        //        SetProperty(ref _validateProductUnits, value);
        //        RaisePropertyChanged(() => ValidateProductUnits);
        //    }
        //}


        //private PickerItem productUnit;
        //public PickerItem ProductUnit
        //{
        //    get => productUnit;
        //    set
        //    {
        //        SetProperty(ref productUnit, value);
        //    }
        //}
        private ValidatableObject<string> _validateName;
        public ValidatableObject<string> ValidateName
        {
            get => _validateName;
            set
            {
                SetProperty(ref _validateName, value);
                RaisePropertyChanged(() => ValidateName);
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
        private string _modifiedByFullname;
        [ModelProperty(nameof(ModelBase.ModifiedByFullName))]
        public string ModifiedByFullName
        {
            get => _modifiedByFullname;
            set { SetProperty(ref _modifiedByFullname, value); }
        }

        private string _createdByFullname;
        [ModelProperty(nameof(ModelBase.CreatedByFullName))]
        public string CreatedByFullName
        {
            get => _createdByFullname;
            set { SetProperty(ref _createdByFullname, value); }
        }

        //private UnitModel _productUnitModel = new UnitModel();
        //[ModelProperty(nameof(ProductModel.ProductUnit))]
        //public UnitModel ProductUnitsModel
        //{
        //    get => _productUnitModel;
        //    set
        //    {
        //        SetProperty(ref _productUnitModel, value);
        //    }
        //}

        private bool _isDelete;
        [ModelProperty(nameof(ModelBase.IsDelete))]
        public bool IsDelete
        {
            get => _isDelete;
            set { SetProperty(ref _isDelete, value); }
        }
        private DelegateCommand _saveProductCommand;
        public DelegateCommand SaveProductCommand => _saveProductCommand ?? (_saveProductCommand = new DelegateCommand(async () => await SaveProductCommandExecute()));

        private DelegateCommand _validateNameCommand;
        public DelegateCommand ValidateNameCommand => _validateNameCommand ?? (_validateNameCommand = new DelegateCommand(() => ValidateNameCommandExecute()));


        private DelegateCommand _openCategoryPickerCommand;
        public DelegateCommand OpenCategoryPickerCommand => _openCategoryPickerCommand ?? (_openCategoryPickerCommand = new DelegateCommand(() => OpenCategoryPickerCommandExecute()));
        private DelegateCommand _openMeasurementTypePickerCommand;
        public DelegateCommand OpenMeasurementTypePickerCommand => _openMeasurementTypePickerCommand ?? (_openMeasurementTypePickerCommand = new DelegateCommand(() => OpenMeasurementTypePickerCommandExecute()));

        private DelegateCommand _validateCategoryCommand;
        public DelegateCommand ValidateCategoryCommand => _validateCategoryCommand ?? (_validateCategoryCommand = new DelegateCommand(() => ValidateProductCategoryCommandExecute()));

        //private DelegateCommand _validateUnitsCommand;
        //public DelegateCommand ValidateUnitsCommand => _validateUnitsCommand ?? (_validateUnitsCommand = new DelegateCommand(() => ValidateProductUnitsCommandExecute()));

        


        public AddProductPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAppSettings settings, IEventAggregator eventAggregator, IProductService productService, IUnitService unitService, ICategoryService categoryService, IMeasurementTypeService measurementTypeService) : base(navigationService, pageDialogService, settings, eventAggregator)
        {
            _productService = productService;
            _measurementTypeService = measurementTypeService;
            _unitService = unitService;
            _categoryService = categoryService;
            ValidateName = new ValidatableObject<string>();
            ValidateProductCategory = new ValidatableObject<PickerItem>();
            ValidateProductMeasurementType=new ValidatableObject<PickerItem>();
            //ValidateProductUnits = new ValidatableObject<PickerItem>();

            //ProductUnit = new PickerItem() { Id = 0, Description = "Select unit", Text = "Select unit" };
            //ValidateProductUnits.Value = ProductUnit;

            ProductCategory = new PickerItem() { Id = 0, Description = "Select Product", Text = "Select Product", PickerItemType = PickerTypesEnums.Products };
            ValidateProductCategory.Value = ProductCategory;

            ProductMeasurementTypePicker = new PickerItem()
            {
                Id = 0,
                Description = "Select measurement type",
                Text = "Select measurement type",
                PickerItemType = PickerTypesEnums.MeasurementType
            };
            ValidateProductMeasurementType.Value = ProductMeasurementTypePicker;
            AddValidations();
        }
        private void AddValidations()
        {
            _validateName.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Name is required"
            });

            _validateProductCategory.Validations.Add(new PickerItemSelectorValidation<PickerItem>() {
                ValidationMessage = "Please select Product"
            });

            _validateProductMeasurementType.Validations.Add(new PickerItemSelectorValidation<PickerItem>()
            {
                ValidationMessage = "Please select measurement type"
            });
            //_validateProductUnits.Validations.Add(new PickerItemSelectorValidation<PickerItem>
            //{
            //    ValidationMessage = "Please select Product"
            //});

            //_validateProductUnits.Validations.Add(new PickerItemSelectorValidation<PickerItem>
            //{
            //    ValidationMessage = "Please select unit"
            //});
        }

        private async void OpenCategoryPickerCommandExecute()
        {
            var CategoryItems = await _categoryService.GetLocalCategoriesModel();
            List<PickerItem> pickerItems = new List<PickerItem>();
            pickerItems = CategoryItems.Select(a => new PickerItem()
            {
                Id = a.LocalId,
                Description = a.Name,
                Text = a.Name,
                PickerItemType = PickerTypesEnums.Categories
            }).ToList();
            await ShowPopup(pickerItems);
        }

        private async void OpenMeasurementTypePickerCommandExecute()
        {
            var MeasurementTypeItems =  _measurementTypeService.GetLocalMeasurementTypesModel();
            List<PickerItem> pickerItems = new List<PickerItem>();
            pickerItems = MeasurementTypeItems.Select(a => new PickerItem()
            {
                Id = a.LocalId,
                Description = a.MeasurementTypeName,
                Text = a.MeasurementTypeName,
                PickerItemType = PickerTypesEnums.MeasurementType
            }).ToList();
            await ShowPopup(pickerItems);
        }
        public AddProductPageViewModel() : base()
        {
            ValidateName = new ValidatableObject<string>();
            ValidateProductCategory = new ValidatableObject<PickerItem>();
            ValidateProductMeasurementType = new ValidatableObject<PickerItem>();
            //ValidateProductUnits = new ValidatableObject<PickerItem>();

            //ProductUnit = new PickerItem() { Id = 0, Description = "Select unit", Text = "Select unit" };
            //ValidateProductUnits.Value = ProductUnit;

            ProductCategory = new PickerItem() { Id = 0, Description = "Select Product", Text = "Select Product", PickerItemType = PickerTypesEnums.Products };

            ValidateProductCategory.Value = ProductCategory;

            ProductMeasurementTypePicker = new PickerItem()
            {
                Id = 0,
                Description = "Select measurement type",
                Text = "Select measurement type",
                PickerItemType= PickerTypesEnums.MeasurementType

            };
            ValidateProductMeasurementType.Value= ProductMeasurementTypePicker;
            AddValidations();
        }

        public async override Task UpdateModel(AddProductPageViewModel model)
        {
            
            this.IsSaved = true;
            model.ProductCategoryModel =await _categoryService.GetLocalCategoryModelByLocalId(model.CategorysId);
            model.ProductMeasurementType = await _measurementTypeService.GetLocalMeasurementTypesModelById(model.MeasurementTypeId);

            //model.ProductUnitsModel = _unitService.GetLocalUnitModelByLocalId(model.UnitsId);
            var ViewModel = await _productService.UpdateProductReturnViewModel<AddProductPageViewModel>(model);

            LocalId = ViewModel.LocalId;
            await NavigationService.GoBackAsync();
        }
        public override AddProductPageViewModel GetData()
        {
            throw new NotImplementedException();
        }
        public override void Update()
        {
            IsActive = true;
            IsDelete = false;
            Operation = Operation.Modified;
            IsEmpty = false;
            ModifyDateTime();
            CreateDateTime();
        }

        private void ModifyDateTime()
        {
            ModifiedAt = DateTime.UtcNow;
            ModifiedBy = _settings.CurrentUser.UserId;
            ModifiedByFullName = _settings.CurrentUser.Username;
        }

        private void CreateDateTime()
        {
            CreatedAt = DateTime.UtcNow;
            CreatedBy = _settings.CurrentUser.UserId;
            CreatedByFullName = _settings.CurrentUser.Username;
        }


        private bool ValidateNameCommandExecute()
        {
            if (_validateName.IsValid)
            {
                Name = ValidateName.Value;
            }
            return _validateName.Validate();
        }
        private async Task SaveProductCommandExecute()
        {
            await ValidateFormFiled();
            if (ValidateName.IsValid &&  ValidateProductCategory.IsValid && ValidateProductMeasurementType.IsValid && ValidateProductCategory.IsValid)
            {
               
                CategorysId = ValidateProductCategory.Value.Id;
                MeasurementTypeId = ValidateProductMeasurementType.Value.Id;
                Update();
                await UpdateModel(this);
            }
        }
        private async Task ValidateFormFiled()
        {

            ValidateNameCommand.Execute();
            ValidateCategoryCommand.Execute();
            //ValidateUnitsCommand.Execute();

        }
        public async override void OnNavigatedFrom(INavigationParameters parameters)
        {
            
            NavigationParameters navigationParameters = new NavigationParameters();
            if (IsSaved == true)
            {
                parameters.Add("NeedRefresh", LocalId);
               
            }
            else
            {
                parameters.Add("NeedRefresh", 0);
               
            }
            base.OnNavigatedFrom(parameters);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    {
                        if (parameters.TryGetValue("SelectedItem", out PickerItem SelectedItem))
                        {

                            //if (SelectedItem.PickerItemType == PickerTypesEnums.Units)
                            //{
                            //    ProductUnit = SelectedItem;
                            //    ValidateProductUnits.Value = SelectedItem;
                            //    ValidateUnitsCommand.Execute();
                            //}
                            if (SelectedItem.PickerItemType == PickerTypesEnums.Categories)
                            {
                                ProductCategory = SelectedItem;
                                ValidateProductCategory.Value = SelectedItem;
                                ValidateCategoryCommand.Execute();

                            }
                            if (SelectedItem.PickerItemType == PickerTypesEnums.MeasurementType)
                            {
                                ProductMeasurementTypePicker = SelectedItem;
                                ValidateProductMeasurementType.Value = SelectedItem;
                                ValidateCategoryCommand.Execute();

                            }
                        }
                        break;
                    }
                case NavigationMode.New:
                    {
                        break;
                    }
                case NavigationMode.Refresh:
                    {
                        break;
                    }
                case NavigationMode.Forward:
                    {
                        break;
                    }

            }
            
        }
        private bool ValidateProductCategoryCommandExecute()
        {
            //if (_validateProductCategory.IsValid)
            //{
            //    CategorysId = ValidateProductCategory.Value.Id;
            //}
            return _validateProductCategory.Validate();

        }
        //private bool ValidateProductUnitsCommandExecute()
        //{
        //    return _validateProductUnits.Validate();
        //}

    }
}
