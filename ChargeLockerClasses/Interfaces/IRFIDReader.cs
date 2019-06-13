using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargeLockerClasses.Interfaces
{
    public class RfidChangedEventArgs : EventArgs
    {
        public string Rfid { get; set; }
    }
    public interface IRFIDReader
    {
        event EventHandler<RfidChangedEventArgs> RfidDetected;
        void Detected();

    }
}
