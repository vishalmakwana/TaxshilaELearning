using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.Models.Requests
{
    public class MobileSyncRequest
    {
        public string Token { get; set; }
        public bool ContinueSync { get; set; }
        public bool ForceNewSync { get; set; }
        public int GroupId { get; set; }
        public int RowCount { get; set; }
        public int SyncTypeId { get; set; }
        public DateTime? LastSyncDate { get; set; }
        public string Username { get; set; }
        public List<int> Ids { get; set; }
        public string ItemIds { get; set; }
    }
}
