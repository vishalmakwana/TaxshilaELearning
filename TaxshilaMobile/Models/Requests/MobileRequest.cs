using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxshilaMobile.Models.Requests
{
    public class MobileRequest
    {
        private DateTime? lastSyncDate;
        public string Token { get; set; }
        public bool ContinueSync { get; set; }
        public bool ForceNewSync { get; set; }
        public int GroupId { get; set; }
        public int RowCount { get; set; }
        public int SyncTypeId { get; set; }

        // Do not set value manually (only by deserializing)!
        public DateTime? LastSyncDate
        {
            get => lastSyncDate?.ToLocalTime();
            set => lastSyncDate = value;
        }

        public string Username { get; set; }
        public List<int> Ids { get; set; }
        public int Id { get; set; }
        public string ItemIds { get; set; }
    }

    
}
