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
    // Drew inspiration for these tests from following link
    // https://stackoverflow.com/questions/12480563/c-sharp-unit-test-a-streamwriter-parameter 

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

        #region LogDoorLocked tests

        [Test]
        public void LogDoorLocked_LoggedForLocked_ContainsText()
        {
            uut.LogDoorLocked(testRfid);
            // Flush method saves the data to the underlying stream
            // in this case the memory stream, which can represent 
            // a file.

            writer.Flush();

            string actual = Encoding.UTF8.GetString(memoryStream.ToArray());

            var contains = actual.Contains("Locker locked with RFID:");

            Assert.AreEqual(contains, true);
        }

        [Test]
        public void LogDoorLocked_LoggedForLocked_WrongText()
        {
            uut.LogDoorLocked(testRfid);
            writer.Flush();

            string actual = Encoding.UTF8.GetString(memoryStream.ToArray());

            var contains = actual.Contains("Locker unlocked with RFID:");

            Assert.AreEqual(contains, false);
        }

        #endregion

        #region LogDoorUnlocked tests

        [Test]
        public void LogDoorUnlocked_LoggedForUnlocked_ContainsText()
        {
            uut.LogDoorUnlocked(testRfid);
            writer.Flush();

            string actual = Encoding.UTF8.GetString(memoryStream.ToArray());

            var contains = actual.Contains("Locker unlocked with RFID:");

            Assert.AreEqual(contains, true);
        }

        [Test]
        public void LogDoorUnlocked_LoggedForUnlocked_WrongText()
        {
            uut.LogDoorUnlocked(testRfid);
            writer.Flush();

            string actual = Encoding.UTF8.GetString(memoryStream.ToArray());

            var contains = actual.Contains("Locker locked with RFID:");

            Assert.AreEqual(contains, false);
        }

        #endregion

    }
}
