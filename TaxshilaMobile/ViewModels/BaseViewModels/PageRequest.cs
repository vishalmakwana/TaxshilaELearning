using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TaxshilaMobile.ViewModels.BaseViewModels
{
    [DebuggerStepThrough]
    public class PageRequest
    {
        public int Id { get; set; }
        public int LocalId { get; set; }
        public int ServerId { get; set; }
      
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string Username { get; set; }
        public bool IsNew { get; set; }
        public int ListId { get; set; }
        public int FavoriteId { get; set; }
        public PageRequest()
        {
            IsNew = false;
        }
    }
}
