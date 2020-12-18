using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TaxshilaMobile.DataTypesApp.Default
{
    public enum MediaAssetType
    {
        Image, Video
    }
    public enum LoginStateTypes : int
    {
        LoggedOut,      // Logged out
        NeverLoggedIn,  // Future use - No instances set up
        LoggedIn        // Currently Logged In
    }
    public enum SortTypes { None, Ascending, Descending }

    public enum SyncStatusTypes : int
    {
        NotStarted = 0,
        Failed,
        Success,
        InProgress
    }
    public enum SyncCategoryTypes : int
    {
        [Description("Units")]
        Units = 0,
        [Description("Categories")]
        Categories = 1,
        [Description("Products")]
        Products = 2
    }

    public enum PickerTypesEnums : int
    {
        [Description("Units")]
        Units = 0,
        [Description("Categories")]
        Categories = 1,
        [Description("Products")]
        Products = 2,
        [Description("MeasurementType")]
        MeasurementType = 3,
        [Description("StockInOutOption")]
        StockInOutOption = 4
    }

    public enum StockInOutEnums : int 
    {
        [Description("Non")]
        Non = 0,
        [Description("StockIn")]
        StockIn = 1,
        [Description("StockOut")]
        StockOut = 2
    }

    //public enum MeasurementTypes : int
    //{
    //    [Description("kg")]
    //    KG = 0,
    //    [Description("lt")]
    //    Liter= 1,
    //    [Description("m")]
    //    Meter = 3,
    //    [Description("bags")]
    //    bags = 4,
    //}
}
