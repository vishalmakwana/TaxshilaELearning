using TaxshilaMobile.DataTypesApp.Default;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.Models.Common
{
    public class SortOrder
    {
        public string ColumnName { get; set; } = "Name";
        public string Title { get; set; } = "Name";
        public SortTypes SortTypes { get; set; } = SortTypes.Ascending;

    }

}
