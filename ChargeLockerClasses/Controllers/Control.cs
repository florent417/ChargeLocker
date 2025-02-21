﻿using System;
using ChargeLockerClasses.Interfaces;

namespace ChargeLockerClasses.Controllers
{
    public class Control
    {
        #region Fields

        private string _oldId;
        private ChargerLockState _lockState;

        private IDoor _door;
        private ICharger _charger;
        private ILogger _logger;
        private IDisplay _display;

        #endregion

        public Control(IDoor door, IRFIDReader rfidReader, 
            ICharger charger, ILogger logger, IDisplay display)
        {
            _door = door;
            _charger = charger;
            _logger = logger;
            _display = display;
            _lockState = ChargerLockState.Available;

            _door.Opened += new EventHandler(DoorOpened);
            _door.Closed += new EventHandler(DoorClosed);

            rfidReader.DetectRfid += new EventHandler<RfidChangedEventArgs>(RfidDetected);
        }

        private void DoorOpened(object sender, EventArgs e)
        {
            switch (_lockState)
            {
                case ChargerLockState.Available:
                    _display.ShowConnectPhone();
                    _lockState = ChargerLockState.DoorOpen;
                    break;
                case ChargerLockState.Locked:
                    // Error message : Locker is locked
                    break;
                case ChargerLockState.DoorOpen:
                    // Error message : Locker is already open
                    break;
            }
            
        }

        private void DoorClosed(object sender, EventArgs e)
        {
            switch (_lockState)
            {
                case ChargerLockState.Available:
                    // Error message : Locker cannot be closed whilst available
                    break;
                case ChargerLockState.Locked:
                    // Error message : Locker is already locked locked
                    break;
                case ChargerLockState.DoorOpen:
                    _display.ShowInputRfid();
                    _lockState = ChargerLockState.Available;
                    break;
            } 
        }

        private bool CheckId(string id)
        {
            if (id == null)
            {
                return false;
            }
            return id.Equals(_oldId);
        }

        // Event handler for the event "RFID Detected" from the state machine diagram for the class
        private void RfidDetected(object sender, RfidChangedEventArgs e)
        {
            switch (_lockState)
            {
                case ChargerLockState.Available:
                    _display.ShowConnectPhone();
                    // Check for charger connection
                    if (_charger.IsConnected())
                    {
                        _door.LockDoor();
                        _charger.StartCharge();
                        _oldId = e.Rfid;
                        _logger.LogDoorLocked(e.Rfid);
                        _display.ShowOccupied();

                        _lockState = ChargerLockState.Locked;
                    }
                    else
                    {
                        _display.ShowConnectionErr();
                    }

                    break;

                case ChargerLockState.DoorOpen:
                    _display.ShowInputRfid();
                    _lockState = ChargerLockState.Available;
                    break;

                case ChargerLockState.Locked:
                    // Check for correct ID
                    if (CheckId(e.Rfid))
                    {
                        _charger.StopCharge();
                        _door.UnlockDoor();
                        _logger.LogDoorUnlocked(e.Rfid);
                        _display.ShowRmvPhone();
                        _lockState = ChargerLockState.Available;
                    }
                    else
                    {
                        _display.ShowRfidErr();
                    }

                    break;
            }
        }

        // Enum with states corresponding ti the statemachine diagram for the class
        private enum ChargerLockState
        {
            Available,
            Locked,
            DoorOpen
        };
    }


}
