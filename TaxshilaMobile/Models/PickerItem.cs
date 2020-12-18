using TaxshilaMobile.DataTypesApp.Default;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.Models
{
    public class PickerItem
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public string ReferenceId { get; set; }
        public PickerTypesEnums PickerItemType { get; set; }
    }
}
