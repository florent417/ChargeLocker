﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargeLockerClasses.Interfaces
{
    public interface ILogger
    {
        void LogDoorLocked(string id);
        void LogDoorUnlocked(string id);

    }
}
