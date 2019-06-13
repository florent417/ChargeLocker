using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChargeLockerClasses.Interfaces;

namespace ChargeLockerClasses.Boundary
{
    public class Logger : ILogger
    {
        private StreamWriter _writer;

        public Logger(StreamWriter writer)
        {
            _writer = writer;
        }

        // Hours, minutes and seconds as the assignment said
        public void LogDoorLocked(string id)
        {
            _writer.WriteLine(DateTime.Now.ToString("HH:mm:ss") + $": Locker locked with RFID: {id}");
        }

        public void LogDoorUnlocked(string id)
        {
            _writer.WriteLine(DateTime.Now.ToString("HH:mm:ss") + $": Locker unlocked with RFID: {id}");
        }
    }
}
