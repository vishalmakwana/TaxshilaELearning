using TaxshilaMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.Validations
{
    public class PickerItemSelectorValidation<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }
        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }


            var SelectPickerItem = value as PickerItem;
            if (SelectPickerItem.Id > 0)
                return true;
            else
                return false;
        }
    }
}
