using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.ServiceBus.OfflineSync
{
    public class CategoryModel : ModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsSaved { get; set; }

        public bool Status { get; set; }


        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<ProductModel> Products { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<StockInOutManagementModel> StockInOutManagements { get; set; }

        //[OneToMany(CascadeOperations = CascadeOperation.All)]
        //public List<UnitModel> Units { get; set; }
    }

    public class ProductModel : ModelBase
    {
        public string Name { get; set; }
        public bool IsSaved { get; set; }

        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool Status { get; set; }
        public string ItemCode { get; set; }

        [ForeignKey(typeof(CategoryModel), Name = nameof(CategorysId))]        
        public int CategorysId { get; set; }

        [ForeignKey(typeof(MeasurementTypeModel), Name = nameof(MeasurementTypeId))]
        public int MeasurementTypeId { get; set; }


        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public MeasurementTypeModel ProductMeasurementType { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public CategoryModel ProductCategory { get; set; }
        public double CostPrice { get; set; }
        public double SalePrice { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<StockInOutManagementModel> StockInOutManagements { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<StockInOutEntryModel> ProductStockInOutEntryModel { get; set; }

        [Ignore]
        public string CategoryName { get 
            {
                return ProductCategory.Name;
            } 
        }

        [Ignore]
        public List<UnitModel> ProductUnitsForReports { get; set; }

    }

    public class UnitModel : ModelBase
    {
        public string UnitTitle { get; set; }
        public bool IsSaved { get; set; }

        [Ignore]
        public int StockCount { get; set; }

        //[ForeignKey(typeof(CategoryModel), Name = nameof(CategorysId))]
        //public int CategorysId { get; set; }

        //[ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        //public CategoryModel UnitCategory { get; set; }

        [ForeignKey(typeof(MeasurementTypeModel), Name = nameof(MeasurementTypeId))]
        public int MeasurementTypeId { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public MeasurementTypeModel MeasurementType { get; set; }
        public int TotalStockIn { get; set; }
        public int TotalStockOut { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<StockInOutEntryModel> StockInOutEntrys { get; set; }

        [Ignore]
        public int TotalStok { get; set; }


    }

    public class SyncStatusModel : ModelBase
    {
        public int Category { get; set; }
        public int SubCategory { get; set; }
        public DateTime? LastSyncedAt { get; set; }
        public int Status { get; set; }
    }

    public class StockInOutManagementModel : ModelBase 
    {

        [ForeignKey(typeof(CategoryModel), Name = nameof(StockInOutCategorysId))]
        public int StockInOutCategorysId { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public CategoryModel StockInOutCategoryModel { get; set; }

        [ForeignKey(typeof(ProductModel), Name = nameof(StockInOutProductId))]
        public int StockInOutProductId { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public ProductModel StockInOutProductModel { get; set; }

        public int StockInOutOption { get; set; }
        public bool IsSaved { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<StockInOutEntryModel> StockInOutEntrys { get; set; }

        [Ignore]
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

    }
    public class StockInOutEntryModel : ModelBase 
    {
        [ForeignKey(typeof(StockInOutManagementModel), Name = nameof(StockInOutManagementId))]
        public int StockInOutManagementId { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public StockInOutManagementModel StockInOutManagement { get; set; }

        [ForeignKey(typeof(UnitModel), Name = nameof(StockInOutUnitId))]
        public int StockInOutUnitId { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public UnitModel StockInOutUnit { get; set; }

        [ForeignKey(typeof(ProductModel), Name = nameof(StockInOutProductId))]
        public int StockInOutProductId { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public ProductModel StockInOutProduct { get; set; }


        public int StockCount { get; set; }
        public int StockInOutOption { get; set; }

        public string UnitTitle { get; set; }



    }
    public class MeasurementTypeModel : ModelBase
    {
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<UnitModel> Units { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<ProductModel> Products { get; set; }
        public string MeasurementTypeName { get; set; }
        public string Descriptions { get; set; }
      
    }


}
