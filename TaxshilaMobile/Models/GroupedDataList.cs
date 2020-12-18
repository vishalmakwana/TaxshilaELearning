using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace TaxshilaMobile.Models
{
    public class GroupedDataList<T> : ObservableCollection<T>, INotifyPropertyChanged
    {
        public string Title { get; set; }
        public int TypeId { get; set; }
        private int _itemCount;
        public int ItemCount
        {
            get { return _itemCount; }
            set
            {
                _itemCount = value;
                OnPropertyChanged("ItemCount");
            }
            //get { return $"{Count} {(Count > 1 ? "cases" : "case")}"; }
        }
        private bool _expanded;
        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    OnPropertyChanged("Expanded");
                    OnPropertyChanged("StateIcon");
                }
            }
        }

        public string StateIcon
        {
            get { return Expanded ? FontAwesome.IconFonts.CaretDown : FontAwesome.IconFonts.CaretRight; }
        }
        public GroupedDataList()
        {

        }
        public GroupedDataList(string title, bool expanded = true)
        {
            Title = title;
            Expanded = expanded;
        }

        public GroupedDataList(int typeID, bool expanded = true)
        {
            TypeId = typeID;
            Expanded = expanded;
        }
        public static ObservableCollection<T> All { private set; get; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
