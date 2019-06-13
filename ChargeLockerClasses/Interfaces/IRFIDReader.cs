using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargeLockerClasses.Interfaces
{
    public interface IRFIDReader
    {
        event EventHandler RfidDetected;
        void Detected();

    }
}
