using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.ServiceBus.Services
{
    public interface IOrientationHandler
    {
        void ForceLandscape();
        void ForcePortrait();
    }
}
