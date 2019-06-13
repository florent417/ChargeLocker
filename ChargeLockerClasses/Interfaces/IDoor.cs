using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ChargeLockerClasses.Interfaces
{
    interface IDoor
    {
        event EventHandler Opened;
        event EventHandler Closed;

        void Open();
        void Close();
        void LockDoor();
        void UnlockDoor();
    }
}
