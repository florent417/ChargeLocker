using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargeLockerClasses.Interfaces
{
    interface IDisplay
    {
        void ShowConnectPhone();
        void ShowInputRfid();
        void ShowConnectionErr();
        void ShowOccupied();
        void ShowRfidErr();
        void ShowRmvPhone();
    }
}
