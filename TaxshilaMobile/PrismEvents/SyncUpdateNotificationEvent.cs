using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.PrismEvents
{
    public class SyncUpdatePayload
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
    public class SyncUpdateNotificationEvent : PubSubEvent<SyncUpdatePayload>
    {

    }

}
