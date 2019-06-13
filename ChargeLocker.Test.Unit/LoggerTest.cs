using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Internal;
using NUnit.Framework;
using Logger = ChargeLockerClasses.Boundary.Logger;

namespace ChargeLocker.Test.Unit
{
    [TestFixture]
    class LoggerTest
    {
        private MemoryStream memoryStream;
        private StreamWriter writer;
        private Logger uut;

        private string testRfid = "A22";

        [SetUp]
        public void Setup()
        {
            memoryStream = new MemoryStream();
            writer = new StreamWriter(memoryStream);
            uut = new Logger(writer);
        }

        [Test]
        public void LogDoorLocked_LockedInfor_ContainsText()
        {
            uut.LogDoorLocked(testRfid);
            writer.Flush();

            string actual = Encoding.UTF8.GetString(memoryStream.ToArray());

            var contains = actual.Contains("Locker locked with RFID:");

            Assert.AreEqual(contains,true);
        }


    }
}
