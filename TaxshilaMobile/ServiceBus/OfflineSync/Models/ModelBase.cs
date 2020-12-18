using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.ServiceBus.OfflineSync
{
    public class ModelBase
    {
        [PrimaryKey, AutoIncrement]
        public int LocalId { get; set; }
        public int ServerId { get; set; } // This is the servers id and must ALWAYS match
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByFullName { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedByFullName { get; set; }
        public int Operation { get; set; }
        public int ApplicationType { get; set; }
        public bool IsEmptyModel { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
    public enum Operation
    {
        None,
        Modified,
        Inserted,
        Deleted,
        Synced
    }
}
