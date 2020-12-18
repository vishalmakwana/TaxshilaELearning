using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TaxshilaMobile.Models
{
    public class Category : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public string SoftColor { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
