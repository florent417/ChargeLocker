public class Control
{
    // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
    private enum ChargerLockState
    {
        Available,
        Locked,
        DoorOpen
    };


    private string logFile = "logfile.txt"; // Navnet på systemets log-fil

    // Eksempel på event handler for eventet "RFID Detected" fra tilstandsdiagrammet for klassen
    private void RfidDetected(int id)
    {
        switch (state)
        {
        case LadeskabState.Available:
            // Check for ladeforbindelse
            if (charger.IsConnected())
            {
                door.LockDoor();
                charger.StartCharge();
                oldId = id;
                using (var writer = File.AppendText(logFile))
                {
                    writer.WriteLine(DateTime.Now + ": Skab låst med RFID: {0}", id);
                }

                Console.Writeline("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
                state = LadeskabState.Locked;
            }
            else
            {
                Console.Writeline("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
            }

            break;

        case LadeskabState.DoorOpen:
            // Ignore
            break;

        case LadeskabState.Locked:
            // Check for correct ID
            if (id == oldId)
            {
                charger.StopCharge();
                door.UnlockDoor();
                using (var writer = File.AppendText(logFile))
                {
                    writer.WriteLine(DateTime.Now + ": Skab låst med RFID: {0}", id);
                }

                Console.Writeline("Tag din telefon ud af skabet og luk døren");
                state = LadeskabState.Available;
            }
            else
            {
                Console.Writeline("Forkert RFID tag");
            }

            break;
        }
    }
}
