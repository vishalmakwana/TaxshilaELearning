using ImTools;
using TaxshilaMobile.Helpers;
using TaxshilaMobile.ServiceBus.OfflineSync.Models.Exceptions;
using TaxshilaMobile.ServiceBus.OfflineSync.Models.ThinViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TaxshilaMobile.ServiceBus.OfflineSync.Models
{
    public static class ModelMapper
    {
        public static UnitModel MapToUnitModel(this UnitModelDTO model, int operation = (int)Operation.Synced)
        {
            return new UnitModel()
            {
                ServerId = model.Id,
                UnitTitle = model.Title,
                Operation = operation,
                IsActive = model.IsActive ?? true,
                IsDelete = model.IsDelete ?? false,
                CreatedAt = model.CreatedAt ?? DateTime.UtcNow,
                ModifiedBy = model.ModifiedBy,
                ModifiedAt = model.ModifiedAt ?? DateTime.UtcNow,
                CreatedBy = model.CreatedBy,
                MeasurementTypeId = model.MeasurementTypeId,
                //Products = model.ProductsDTO != null ? new List<ProductModel>(model.ProductsDTO.Select(x => x.MapToProductModel()).ToList()) : null
            };
        }

        public static UnitModelDTO MapToUnitModelDTO(this UnitModel model)
        {
            return new UnitModelDTO()
            {
                Id = model.ServerId,
                IsDelete = model.IsDelete,
                IsActive = model.IsActive,
                LocalId = model.LocalId,
                CreatedAt = model.CreatedAt,
                ModifiedBy = model.ModifiedBy,
                ModifiedAt = model.ModifiedAt,
                CreatedBy = model.CreatedBy,
                IsEmptyModel = false,
                MeasurementTypeId = model.MeasurementTypeId,

            };
        }

        public static ProductModel MapToProductModel(this ProductModelDTO modelDto, int operation = (int)Operation.Synced)
        {
            return new ProductModel()
            {
                ServerId = modelDto.Id,
                Name = modelDto.Name,
                Operation = operation,
                IsActive = modelDto.IsActive ?? true,
                IsDelete = modelDto.IsDelete ?? false,
                Description = modelDto.Description,
                ImageUrl = modelDto.ImageUrl,
                Status = modelDto.Status ?? true,
                ItemCode = modelDto.ItemCode,
                CategorysId = modelDto.CategorysId,

                SalePrice = modelDto.SalePrice,
                CostPrice = modelDto.CostPrice,
                CreatedAt = modelDto.CreatedAt ?? DateTime.UtcNow,
                ModifiedAt = modelDto.ModifiedAt ?? DateTime.UtcNow,
                CreatedBy = modelDto.CreatedBy,
                ModifiedBy = modelDto.ModifiedBy,
                IsSaved = true,
                IsEmptyModel = false,


            };
        }



        public static ProductModelDTO MapToProductDTO(this ProductModel productModel)
        {
            return new ProductModelDTO()
            {
                Id = productModel.ServerId,
                LocalId = productModel.LocalId,
                IsDelete = productModel.IsDelete,
                IsActive = productModel.IsActive,
                Name = productModel.Name,
                Description = productModel.Description,
                ImageUrl = productModel.ImageUrl,
                Status = productModel.Status,
                ItemCode = productModel.ItemCode,
                CategorysId = productModel.CategorysId,

                SalePrice = productModel.SalePrice,
                CostPrice = productModel.CostPrice,
                CreatedAt = productModel.CreatedAt,
                ModifiedAt = productModel.ModifiedAt,
                CreatedBy = productModel.CreatedBy,
                ModifiedBy = productModel.ModifiedBy,
                IsEmptyModel = false
            };
        }


        public static CategoryModel MapToCategoryModel(this CategoryModelDTO modelDto, int operation = (int)Operation.Synced)
        {
            return new CategoryModel()
            {
                ServerId = modelDto.Id,
                Name = modelDto.Name,
                IsActive = modelDto.IsActive ?? true,
                IsDelete = modelDto.IsDelete ?? false,
                Description = modelDto.Description,
                IsFeatured = modelDto.IsFeatured ?? true,
                ImageUrl = modelDto.ImageUrl,
                Status = modelDto.Status ?? true,
                Operation = operation,
                CreatedAt = modelDto.CreatedAt ?? DateTime.UtcNow,
                CreatedBy = modelDto.CreatedBy,
                ModifiedAt = modelDto.ModifiedAt ?? DateTime.UtcNow,
                ModifiedBy = modelDto.ModifiedBy,
                Products = modelDto.ProductsDTO.AnyExtended() ? modelDto.ProductsDTO.Select(a => a.MapToProductModel()).ToList() : null,
            };
        }

        public static CategoryModelDTO MapToCategoryDTO(this CategoryModel categorymodel)
        {
            return new CategoryModelDTO()
            {
                Id = categorymodel.ServerId,
                LocalId = categorymodel.LocalId,
                IsDelete = categorymodel.IsDelete,
                IsActive = categorymodel.IsActive,
                Name = categorymodel.Name,
                Description = categorymodel.Description,
                ImageUrl = categorymodel.ImageUrl,
                Status = categorymodel.Status,
                IsFeatured = categorymodel.IsFeatured,
                ProductsDTO = categorymodel.Products.AnyExtended() ? categorymodel.Products.Select(a => a.MapToProductDTO()).ToList() : null,
            };

        }

    }

    public class BaseMapper
    {
        public TEntity MapViewModelToModel<TEntity>(object viewModel) where TEntity : class, new()
        {
            return ToModel<TEntity>(viewModel);
        }

        /// <summary>
        /// Creates a Model from the ViewModel
        /// </summary>
        /// <typeparam name="ModelType"></typeparam>
        /// <typeparam name="ViewModelType"></typeparam>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public ModelType ToModel<ModelType>(object viewModel) where ModelType : new()
        {

            ModelType model = new ModelType();

            try
            {
                var props = DehydrateObject<ModelPropertyAttribute>(viewModel);  // Find all attributes with ModelPropertyAttribute
                foreach (PropertyInfo info in props)
                {
                    // Find PropertyName on ViewModel, currentVaalue in ViewModel, and the CurrrentValue in the Model
                    var targetPropertyName = info.GetCustomAttribute(typeof(ModelPropertyAttribute)) as ModelPropertyAttribute;
                    var currentVal = info.GetValue(viewModel);
                    var targetVal = model.GetType().GetProperty(targetPropertyName.PropertyName).GetValue(model);

                    //if (targetPropertyName.PropertyName == "CaseAction") Debugger.Break();
                    if (currentVal != targetVal)
                    {
                        if (info.PropertyType.IsEnum)
                        {
                            var prop = model.GetType().GetProperty(info.Name);
                            //System.Type enumUnderlyingType = System.Enum.GetUnderlyingType(info);
                            //int caseAct = (int)targetVal.ToString();
                            prop.SetValue(model, Convert.ToInt32(currentVal));
                        }
                        else if (info.PropertyType.Name.Contains("ObservableCollection") || info.PropertyType.Name.Contains("ObservableRangeCollection"))
                        {
                            PropertyInfo modelProp = model.GetType().GetProperty(targetPropertyName.PropertyName);
                            var genericArg = modelProp.PropertyType.GenericTypeArguments.FirstOrDefault(); ;

                            if (targetVal == null)
                            {
                                var listType = typeof(List<>);
                                var constructedListType = listType.MakeGenericType(genericArg);
                                IList tempList = (IList)Activator.CreateInstance(constructedListType);
                                var list = GetThinListForModelCollection(currentVal as IEnumerable, genericArg);
                                var addMethod = modelProp.PropertyType.GetMethod("Add");
                                object instanceValue = modelProp.GetValue(model);
                                if (instanceValue == null)
                                {
                                    var tempListType = typeof(List<>);
                                    // var constructedListType = listType.MakeGenericType(modelProp.);
                                    instanceValue = Activator.CreateInstance(constructedListType);
                                    modelProp.SetValue(model, instanceValue);
                                }
                                foreach (var item in list)
                                {
                                    addMethod.Invoke(instanceValue, new object[] { item });
                                    //tempList.Add(item);
                                }

                                //var childd = model.GetType().GetProperty("Children");
                                //var addChild = modelProp.PropertyType.GetMethod("Add");
                                //modelProp.SetValue(model, tempList);
                                //targetVal = model.GetType().GetProperty(targetPropertyName.PropertyName).GetValue(model);
                            }

                            // fix for model
                            //    var list = GetThinListForCollection(currentVal as IEnumerable, genericArg);
                            //PropertyInfo modelListProp = model.GetType().GetProperty(targetPropertyName.PropertyName);
                            //modelListProp.PropertyType.GetMethod("ReplaceRange").Invoke(targetVal, new[] { list });

                        }
                        else
                        {
                            PropertyInfo modelProp = model.GetType().GetProperty(targetPropertyName.PropertyName);
                            modelProp.SetValue(model, ChangeType(currentVal, modelProp.PropertyType));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in ToModel: ex: " + ex);
            }
            return model;
        }

        /// <summary>
        /// Create a ThinViewModel for the Model collection
        /// </summary>
        /// <param name="source"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public IEnumerable GetThinListForModelCollection(System.Collections.IEnumerable source, Type targetType)
        {
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(targetType);
            IList list = (IList)Activator.CreateInstance(constructedListType);
            foreach (var item in source)
            {
                var newObject = Activator.CreateInstance(targetType);

                PopulateModel(item, newObject);

                list.Add(newObject);
            }
            return list;
        }
        /// <summary>
        /// Conversion
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public T ChangeType<T>(object value)
        {
            var t = typeof(T);

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return default(T);
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return (T)Convert.ChangeType(value, t);
        }
        /// <summary>
        /// Conversion
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversion"></param>
        /// <returns></returns>
        public object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }
        /// <summary>
        /// Upate the view model from a model
        /// </summary>
        /// <param name="parentObject"></param>
        /// <param name="currentModel"></param>
        public void PopulateModel(object parentObject, object currentModel)
        {
            var props = DehydrateObject<ModelPropertyAttribute>(parentObject);
            foreach (PropertyInfo info in props)
            {
                var targetPropertyName = info.GetCustomAttribute(typeof(ModelPropertyAttribute)) as ModelPropertyAttribute;

                // if (info.Name == "Notes") Debugger.Break();// use this to debug a collection
                try
                {
                    var currentVal = info.GetValue(parentObject);
                    var targetVal = currentModel.GetType().GetProperty(targetPropertyName.PropertyName).GetValue(currentModel);

                    if (currentVal != targetVal)
                    {
                        // if it's an observable collection
                        if (info.PropertyType.Name.Contains("ObservableCollection") || info.PropertyType.Name.Contains("ObservableRangeCollection"))
                        {
                            PropertyInfo modelProp = currentModel.GetType().GetProperty(targetPropertyName.PropertyName);
                            var genericArg = modelProp.PropertyType.GenericTypeArguments.FirstOrDefault();

                            if (targetVal == null)
                            {
                                var listType = typeof(List<>);
                                var constructedListType = listType.MakeGenericType(genericArg);
                                IList tempList = (IList)Activator.CreateInstance(constructedListType);
                                var list = GetThinListForModelCollection(currentVal as IEnumerable, genericArg);
                                var addMethod = modelProp.PropertyType.GetMethod("Add");
                                object instanceValue = modelProp.GetValue(currentModel);
                                if (instanceValue == null)
                                {
                                    var tempListType = typeof(List<>);
                                    instanceValue = Activator.CreateInstance(constructedListType);
                                    modelProp.SetValue(currentModel, instanceValue);
                                }

                                foreach (var item in list)
                                {
                                    addMethod.Invoke(instanceValue, new object[] { item });
                                }
                            }
                            else
                            {
                                genericArg = info.PropertyType.GenericTypeArguments.FirstOrDefault();
                                var list = GetThinListForCollection(currentVal as IEnumerable, genericArg);
                                info.PropertyType.GetMethod("ReplaceRange").Invoke(targetVal, new[] { list });
                            }
                        }
                        else // set object normally
                        {
                            var newProp = currentModel.GetType().GetProperty(targetPropertyName.PropertyName);
                            newProp.SetValue(currentModel, currentVal);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ConvertFromModelToViewModelException(ex.Message, info.Name, targetPropertyName.PropertyName, null);
                }
            }
        }
        public TViewModel MapToViewModel<TViewModel>(object model) where TViewModel : new()
        {

            //TEntity list = (TEntity)Activator.CreateInstance(typeof(TEntity));

            TViewModel viewModel = new TViewModel();

            PopulateViewModel(viewModel, model);

            return viewModel;


        }

        /// <summary>
        /// Get all properties of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<PropertyInfo> DehydrateObject<T>(object obj)
        {
            List<PropertyInfo> values =
                (from property in obj.GetType().GetProperties()
                 where property.GetCustomAttributes(typeof(T), false).Length > 0
                 select property).ToList();

            return values;
        }

        /// <summary>
        /// Upate the view model from a model
        /// </summary>
        /// <param name="parentObject"></param>
        /// <param name="currentModel"></param>
        public void PopulateViewModel(object parentObject, object currentModel)
        {
            var props = DehydrateObject<ModelPropertyAttribute>(parentObject);
            foreach (PropertyInfo info in props)
            {
                var targetPropertyName = info.GetCustomAttribute(typeof(ModelPropertyAttribute)) as ModelPropertyAttribute;

                //if (info.Name == "AssignedToAt") Debugger.Break();// use this to debug a collection
                try
                {
                    var currentVal = info.GetValue(parentObject);
                    var a1 = currentModel.GetType();
                    var a2 = currentModel.GetType().GetProperty(targetPropertyName.PropertyName);


                    var targetVal = currentModel.GetType().GetProperty(targetPropertyName.PropertyName).GetValue(currentModel);

                    if (currentVal != targetVal)
                    {

                        // if it's an observable collection
                        if (info.PropertyType.Name.Contains("ObservableCollection") || info.PropertyType.Name.Contains("ObservableRangeCollection"))
                        {
                            var genericArg = info.PropertyType.GenericTypeArguments.FirstOrDefault();

                            var list = GetThinListForCollection(targetVal as IEnumerable, genericArg);
                            info.PropertyType.GetMethod("ReplaceRange").Invoke(currentVal, new[] { list });

                        }
                        else if (info.PropertyType.IsSubclassOf(typeof(BaseThinViewModel)))
                        {
                            // if type is a thin model
                            var item = GetThinViewModel(info.PropertyType, targetVal);
                            info.SetValue(parentObject, item);
                        }
                        else // set object normally
                        {
                            info.SetValue(parentObject, targetVal);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ConvertFromModelToViewModelException(ex.Message, info.Name, targetPropertyName.PropertyName, null);
                }
            }
        }

        /// <summary>
        /// Create a ThinViewModel for the Model collection
        /// </summary>
        /// <param name="source"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public IEnumerable GetThinListForCollection(System.Collections.IEnumerable source, Type targetType)
        {
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(targetType);
            IList list = (IList)Activator.CreateInstance(constructedListType);
            if (source != null)
            {
                foreach (var item in source)
                {
                    var newObject = Activator.CreateInstance(targetType);

                    PopulateViewModel(newObject, item);

                    list.Add(newObject);
                }
            }

            return list;
        }
        public BaseThinViewModel GetThinViewModel(Type targetType, object model)
        {
            //try
            //{
            BaseThinViewModel newObject = Activator.CreateInstance(targetType) as BaseThinViewModel;


            PopulateViewModel(newObject, model);
            return newObject;
            //} catch ( Exception ex)
            //{

            //}
        }


    }

    public class CategoryMapper : BaseMapper, IMapper<CategoryModelDTO, CategoryModel>
    {
        public CategoryModelDTO Map(CategoryModel obj)
        {
            return new CategoryModelDTO()
            {
                Id = obj.ServerId,
                LocalId = obj.LocalId,
                IsDelete = obj.IsDelete,
                IsActive = obj.IsActive,
                Name = obj.Name,
                Description = obj.Description,
                ImageUrl = obj.ImageUrl,
                Status = obj.Status,
                IsFeatured = obj.IsFeatured,
                ProductsDTO = obj.Products.AnyExtended() ? obj.Products.Select(a => a.MapToProductDTO()).ToList() : null,
            };
        }

        public CategoryModel Map(CategoryModelDTO obj)
        {
            return new CategoryModel()
            {
                ServerId = obj.Id,
                Name = obj.Name,
                IsActive = obj.IsActive ?? true,
                IsDelete = obj.IsDelete ?? false,
                Description = obj.Description,
                IsFeatured = obj.IsFeatured ?? true,
                ImageUrl = obj.ImageUrl,
                Status = obj.Status ?? true,
                CreatedAt = obj.CreatedAt ?? DateTime.UtcNow,
                CreatedBy = obj.CreatedBy,
                ModifiedAt = obj.ModifiedAt ?? DateTime.UtcNow,
                ModifiedBy = obj.ModifiedBy,
                Products = obj.ProductsDTO.AnyExtended() ? obj.ProductsDTO.Select(a => a.MapToProductModel()).ToList() : null,
            };
        }

        public CategoryModel Map(CategoryModelDTO obj, int Operation)
        {
            return new CategoryModel()
            {
                ServerId = obj.Id,
                Name = obj.Name,
                IsActive = obj.IsActive ?? true,
                IsDelete = obj.IsDelete ?? false,
                Description = obj.Description,
                IsFeatured = obj.IsFeatured ?? true,
                ImageUrl = obj.ImageUrl,
                Status = obj.Status ?? true,
                CreatedAt = obj.CreatedAt ?? DateTime.UtcNow,
                CreatedBy = obj.CreatedBy,
                ModifiedAt = obj.ModifiedAt ?? DateTime.UtcNow,
                ModifiedBy = obj.ModifiedBy,
                Operation = Operation,
                Products = obj.ProductsDTO.AnyExtended() ? obj.ProductsDTO.Select(a => a.MapToProductModel()).ToList() : null,
            };
        }
    }

    public class StocInOutMapper : BaseMapper, IMapper<StockInOutDTO, StockInOutManagementModel>
    {
        public StockInOutDTO Map(StockInOutManagementModel obj)
        {
            return new StockInOutDTO()
            {
                Id = obj.ServerId,
                LocalId = obj.LocalId,
                StockInOutCategorysId = obj.StockInOutCategorysId,
                CreatedAt = obj.CreatedAt,
                CreatedBy = obj.CreatedBy,
                ModifiedAt = obj.ModifiedAt,
                ModifiedBy = obj.ModifiedBy,
             
                IsEmptyModel = false,
                StockInOutProductId = obj.StockInOutProductId,
                IsActive = obj.IsActive,
                IsDelete = obj.IsDelete
            };
        }

        public StockInOutManagementModel Map(StockInOutDTO obj)
        {
            return new StockInOutManagementModel()
            {
                CreatedAt = obj.CreatedAt ?? DateTime.UtcNow,
                CreatedBy = obj.CreatedBy,
               
                IsActive = obj.IsActive ?? true,
                StockInOutCategorysId = obj.StockInOutCategorysId,
               
                StockInOutProductId = obj.StockInOutProductId,
                ModifiedAt = obj.ModifiedAt ?? DateTime.UtcNow,
                ModifiedBy = obj.ModifiedBy,
                ServerId = obj.Id,
                Operation = (int)Operation.Inserted,
               
                IsDelete = obj.IsDelete ?? false,
                IsEmptyModel = false
            };
        }

        public StockInOutManagementModel Map(StockInOutDTO obj, int Operation)
        {
            return new StockInOutManagementModel()
            {
                CreatedAt = obj.CreatedAt ?? DateTime.UtcNow,
                CreatedBy = obj.CreatedBy,
               
                IsActive = obj.IsActive ?? true,
                StockInOutCategorysId = obj.StockInOutCategorysId,
               
                StockInOutProductId = obj.StockInOutProductId,
                ModifiedAt = obj.ModifiedAt ?? DateTime.UtcNow,
                ModifiedBy = obj.ModifiedBy,
                ServerId = obj.Id,
                Operation = Operation,
                IsDelete = obj.IsDelete ?? false,
                IsEmptyModel = false
            };
        }
    }

    public class ProductsMapper : BaseMapper, IMapper<ProductModelDTO, ProductModel>
    {
        public ProductModelDTO Map(ProductModel obj)
        {
            return new ProductModelDTO()
            {
                Id = obj.ServerId,
                LocalId = obj.LocalId,
                IsDelete = obj.IsDelete,
                IsActive = obj.IsActive,
                Name = obj.Name,
                Description = obj.Description,
                ImageUrl = obj.ImageUrl,
                Status = obj.Status,
                ItemCode = obj.ItemCode,
                CategorysId = obj.CategorysId,
                SalePrice = obj.SalePrice,
                CostPrice = obj.CostPrice,
                CreatedAt = obj.CreatedAt,
                ModifiedAt = obj.ModifiedAt,
                CreatedBy = obj.CreatedBy,
                ModifiedBy = obj.ModifiedBy,
                IsEmptyModel = false
            };
        }

        public ProductModel Map(ProductModelDTO obj)
        {
            return new ProductModel()
            {
                ServerId = obj.Id,
                Name = obj.Name,
                IsActive = obj.IsActive ?? true,
                IsDelete = obj.IsDelete ?? false,
                Description = obj.Description,
                ImageUrl = obj.ImageUrl,
                Status = obj.Status ?? true,
                ItemCode = obj.ItemCode,
                CategorysId = obj.CategorysId,

                SalePrice = obj.SalePrice,
                CostPrice = obj.CostPrice,
                CreatedAt = obj.CreatedAt ?? DateTime.UtcNow,
                ModifiedAt = obj.ModifiedAt ?? DateTime.UtcNow,
                ModifiedBy = obj.ModifiedBy,
                CreatedBy = obj.CreatedBy,
                Operation = (int)Operation.Synced,
                IsSaved = true,
                IsEmptyModel = false,



            };
        }

        public ProductModel Map(ProductModelDTO obj, int Operation)
        {
            return new ProductModel()
            {
                ServerId = obj.Id,
                Name = obj.Name,
                IsActive = obj.IsActive ?? true,
                IsDelete = obj.IsDelete ?? false,
                Description = obj.Description,
                ImageUrl = obj.ImageUrl,
                Status = obj.Status ?? true,
                ItemCode = obj.ItemCode,
                CategorysId = obj.CategorysId,

                SalePrice = obj.SalePrice,
                CostPrice = obj.CostPrice,
                CreatedAt = obj.CreatedAt ?? DateTime.UtcNow,
                ModifiedAt = obj.ModifiedAt ?? DateTime.UtcNow,
                ModifiedBy = obj.ModifiedBy,
                CreatedBy = obj.CreatedBy,
                Operation = Operation,
                IsSaved = true,
                IsEmptyModel = false,
            };
        }

    }
    public class UnitsMapper : BaseMapper, IMapper<UnitModelDTO, UnitModel>
    {
        public UnitModelDTO Map(UnitModel unitModel)
        {
            return new UnitModelDTO()
            {
                Id = unitModel.ServerId,
                LocalId = unitModel.LocalId,
                Title = unitModel.UnitTitle,
                IsActive = unitModel.IsActive,
                IsDelete = unitModel.IsDelete,
                CreatedAt = unitModel.CreatedAt,
                ModifiedBy = unitModel.ModifiedBy,
                ModifiedAt = unitModel.ModifiedAt,
                CreatedBy = unitModel.CreatedBy,
                MeasurementTypeId = unitModel.MeasurementTypeId,
            };
        }

        public UnitModel Map(UnitModelDTO obj)
        {
            return new UnitModel()
            {
                ServerId = obj.Id,
                UnitTitle = obj.Title,
                IsActive = obj.IsActive ?? true,
                IsDelete = obj.IsDelete ?? false,
                CreatedAt = obj.CreatedAt ?? DateTime.UtcNow,
                ModifiedBy = obj.ModifiedBy,
                ModifiedAt = obj.ModifiedAt ?? DateTime.UtcNow,
                CreatedBy = obj.CreatedBy,
                MeasurementTypeId = obj.MeasurementTypeId,

                //Products = obj.ProductsDTO != null ? new List<ProductModel>(obj.ProductsDTO.Select(x => x.MapToProductModel()).ToList()) : null
            };
        }

        public UnitModel Map(UnitModelDTO obj, int Operation)
        {
            return new UnitModel()
            {
                ServerId = obj.Id,
                Operation = Operation,
                UnitTitle = obj.Title,
                IsActive = obj.IsActive ?? true,
                IsDelete = obj.IsDelete ?? false,
                CreatedAt = obj.CreatedAt ?? DateTime.UtcNow,
                ModifiedBy = obj.ModifiedBy,
                ModifiedAt = obj.ModifiedAt ?? DateTime.UtcNow,
                CreatedBy = obj.CreatedBy,
                MeasurementTypeId = obj.MeasurementTypeId,

                //Products = obj.ProductsDTO != null ? new List<ProductModel>(obj.ProductsDTO.Select(x => x.MapToProductModel()).ToList()) : null
            };
        }
    }

   

}
