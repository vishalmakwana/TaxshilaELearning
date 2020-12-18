using TaxshilaMobile.ServiceBus.OfflineSync;
using TaxshilaMobile.ServiceBus.OfflineSync.Models.ThinViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.Validations
{
    public class UnitSelectorValidation<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }
        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }


            var selectedUnitModel = value as UnitModel;
            if (selectedUnitModel.LocalId > 0)
                return true;
            else
                return false;

        }
    }
}
