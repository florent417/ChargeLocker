

using System;
using ChargeLockerClasses.Interfaces;

namespace ChargeLockerClasses.Controllers
{
    public class Control
    {
        #region Fields

        private string _oldId;
        private ChargerLockState _lockState;

        private IDoor _door;
        private IRFIDReader _rfidReader;
        private ICharger _charger;
        private ILogger _logger;
        private IDisplay _display;

        #endregion

        public Control(IDoor door, IRFIDReader rfidReader, 
            ICharger charger, ILogger logger, IDisplay display)
        {
            _door = door;
            _rfidReader = rfidReader;
            _charger = charger;
            _logger = logger;
            _display = display;
            _lockState = ChargerLockState.Available;

            _door.Opened += new EventHandler(DoorOpened);
            _door.Closed += new EventHandler(DoorClosed);

            _rfidReader.RfidDetected += new EventHandler(RfidDetected());
        }

        private void DoorOpened(object sender, EventArgs e)
        {

        }

        private void DoorClosed(object sender, EventArgs e)
        {

        }

        private bool CheckId(string id)
        {
            return id.Equals(_oldId);
        }

        // private string logFile = "logfile.txt"; // Navnet på systemets log-fil

        // Eksempel på event handler for eventet "RFID Detected" fra tilstandsdiagrammet for klassen
        public void RfidDetected(object sender, EventArgs e)
        {
            switch (_lockState)
            {
                case ChargerLockState.Available:
                    _display.ShowConnectPhone();
                    // Check for ladeforbindelse
                    if (_charger.IsConnected())
                    {
                        _door.LockDoor();
                        _charger.StartCharge();
                        _oldId = id;
                        _logger.LogDoorLocked(id);
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
                    if (CheckId(id))
                    {
                        _charger.StopCharge();
                        _door.UnlockDoor();
                        _logger.LogDoorUnlocked(id);
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

        // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
        private enum ChargerLockState
        {
            Available,
            Locked,
            DoorOpen
        };
    }


}
