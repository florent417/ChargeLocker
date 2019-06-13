using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChargeLockerClasses.Controllers;
using ChargeLockerClasses.Interfaces;
using NUnit.Framework;
using NSubstitute;

namespace ChargeLocker.Test.Unit
{
    [TestFixture]
    class ControlTest
    {
        private Control uut;

        private ICharger charger;
        private IDisplay display;
        private IDoor door;
        private ILogger logger;
        private IRFIDReader rfidReader;

        [SetUp]
        public void Setup()
        {
            charger = Substitute.For<ICharger>();
            display = Substitute.For<IDisplay>();
            door = Substitute.For<IDoor>();
            logger = Substitute.For<ILogger>();
            rfidReader = Substitute.For<IRFIDReader>();

            uut = new Control(door,rfidReader,
                charger,logger,display);
        }


    }
}
