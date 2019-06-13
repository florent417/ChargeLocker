using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargeLockerClasses.Interfaces
{
    public interface ICharger
    {
        bool IsConnected();
        void StartCharge();
        void StopCharge();
    }
}
