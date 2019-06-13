﻿using System;
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

        #region DoorOpened tests

        [Test]
        public void DoorOpened_StateAvailable_DisplaysConnect()
        {
            door.Opened += Raise.EventWith(this,EventArgs.Empty);

            display.Received(1).ShowConnectPhone();
        }


        #endregion

        #region DoorClosed tests

        [Test]
        public void DoorClose_StateOpen_DisplaysInputRfid()
        {
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);

            display.Received(1).ShowInputRfid();
        }


        #endregion

        #region RfidDetected tests

        #region RfidDetected State = Available

        [Test]
        public void RfidDetected_StateAvailable_DisplaysConnectPhone()
        {
            rfidReader.DetectRfid += Raise.EventWith(this, new RfidChangedEventArgs());

            display.Received(1).ShowConnectPhone();
        }

        [Test]
        public void RfidDetected_StateAvailable_ChargerIsConnected()
        {
            rfidReader.DetectRfid += Raise.EventWith(this, new RfidChangedEventArgs());

            charger.Received(1).IsConnected();
        }

        [Test]
        public void RfidDetected_StateAvailable_ChargerIsNotConnected()
        {
            rfidReader.DetectRfid += Raise.EventWith(this, new RfidChangedEventArgs());

            charger.IsConnected().Returns(false);

            charger.Received(1).IsConnected();
        }

        [Test]
        public void RfidDetected_StateAvailable_DisplaysConnectionErr()
        {
            rfidReader.DetectRfid += Raise.EventWith(this, new RfidChangedEventArgs());

            charger.IsConnected().Returns(false);

            display.Received(1).ShowConnectionErr();
        }

        [Test]
        public void RfidDetected_StateAvailable_DoesntDisplayShowOccupied()
        {
            rfidReader.DetectRfid += Raise.EventWith(this, new RfidChangedEventArgs());

            charger.IsConnected().Returns(false);

            display.DidNotReceive().ShowOccupied();
        }

        [Test]
        public void RfidDetected_StateAvailable_LockDoorCalled()
        {
            charger.IsConnected().Returns(true);
            rfidReader.DetectRfid += Raise.EventWith(this, new RfidChangedEventArgs());

            door.Received(1).LockDoor();
        }

        [Test]
        public void RfidDetected_StateAvailable_StartChargeCalled()
        {
            charger.IsConnected().Returns(true);
            rfidReader.DetectRfid += Raise.EventWith(this, new RfidChangedEventArgs());

            charger.Received(1).StartCharge();
        }

        [Test]
        public void RfidDetected_StateAvailable_LogDoorLockedCalled()
        {
            charger.IsConnected().Returns(true);
            rfidReader.DetectRfid += Raise.EventWith(this, new RfidChangedEventArgs());

            logger.Received(1).LogDoorLocked(null);
        }



        #endregion

        #region RfidDetected State = DoorOpen

        [Test]
        public void RfidDetected_StateOpen_DisplaysInputRfid()
        {
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            rfidReader.DetectRfid += Raise.EventWith(this, new RfidChangedEventArgs { Rfid = "22"});

            display.Received(1).ShowInputRfid();
        }

        [Test]
        public void RfidDetected_StateOpen_DoesntDisplayWrongInfo()
        {
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            rfidReader.DetectRfid += Raise.EventWith(this, new RfidChangedEventArgs { Rfid = "22" });

            display.DidNotReceive().ShowOccupied();
        }



        #endregion


        #endregion


    }
}
