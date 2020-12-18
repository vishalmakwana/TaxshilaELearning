using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.ServiceBus.OfflineSync.Models.ThinViewModels
{
    public class UnitsThinViewModel : BaseThinViewModel
    {
        private string _unittitle;
        [ModelProperty(nameof(UnitModel.UnitTitle))]
        public string UnitTitle
        {
            get => _unittitle;
            set { SetProperty(ref _unittitle, value); }
        }

        private int _measurementTypeId;
        [ModelProperty(nameof(UnitModel.MeasurementTypeId))]
        public int MeasurementTypeId
        {
            get => _measurementTypeId;
            set { SetProperty(ref _measurementTypeId, value); }
        }

        private MeasurementTypeModel _unitMeasurementType = new MeasurementTypeModel();
        [ModelProperty(nameof(UnitModel.MeasurementType))]
        public MeasurementTypeModel UnitMeasurementType
        {
            get => _unitMeasurementType;
            set { SetProperty(ref _unitMeasurementType, value); }
        }


        private bool _isSaved;
        [ModelProperty(nameof(UnitModel.IsSaved))]
        public bool IsSaved
        {
            get => _isSaved;
            set
            {
                SetProperty(ref _isSaved, value);               
            }
        }

        private int _totalStockIn;
        [ModelProperty(nameof(UnitModel.TotalStockIn))]
        public int TotalStockIn
        {
            get => _totalStockIn;
            set { SetProperty(ref _totalStockIn, value); }
        }
        private int _totalStockOut;
        [ModelProperty(nameof(UnitModel.TotalStockOut))]
        public int TotalStockOut
        {
            get => _totalStockOut;
            set { SetProperty(ref _totalStockOut, value); }
        }

        private List<StockInOutEntryModel> _stockInOutEntrys = new List<StockInOutEntryModel>();
        [ModelProperty(nameof(UnitModel.StockInOutEntrys))]
        public List<StockInOutEntryModel> StockInOutEntrys
        {
            get => _stockInOutEntrys;
            set { SetProperty(ref _stockInOutEntrys, value); }
        }
        
    }
}
