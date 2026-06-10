using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define paths
            string soundFilePath = "reminder.wav";
            string calendarMsgPath = "appointment.msg";

            // Ensure the sound file exists; create a minimal WAV placeholder if missing
            if (!File.Exists(soundFilePath))
            {
                try
                {
                    // Minimal 1‑second silent WAV (44‑byte header, no data)
                    byte[] wavHeader = new byte[]
                    {
                        0x52,0x49,0x46,0x46,0x24,0x00,0x00,0x00,0x57,0x41,0x56,0x45,
                        0x66,0x6D,0x74,0x20,0x10,0x00,0x00,0x00,0x01,0x00,0x01,0x00,
                        0x40,0x1F,0x00,0x00,0x80,0x3E,0x00,0x00,0x02,0x00,0x10,0x00,
                        0x64,0x61,0x74,0x61,0x00,0x00,0x00,0x00
                    };
                    File.WriteAllBytes(soundFilePath, wavHeader);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder sound file: {ex.Message}");
                    return;
                }
            }

            // Create a MAPI calendar item and set the custom sound reminder
            using (MapiCalendar calendar = new MapiCalendar())
            {
                // Use StartDate and EndDate properties (StartTime/EndTime are not available)
                calendar.StartDate = DateTime.Now.AddHours(1);
                calendar.EndDate   = DateTime.Now.AddHours(2);
                calendar.Subject   = "Team Meeting";
                calendar.Location  = "Conference Room";

                // Set the full path of the sound file to be played when the reminder fires
                calendar.ReminderFileParameter = Path.GetFullPath(soundFilePath);
                // Enable the sound reminder
                calendar.SetProperty(KnownPropertyList.ReminderPlaySound, true);

                // Save the calendar as a MSG file
                try
                {
                    calendar.Save(calendarMsgPath);
                    Console.WriteLine($"Calendar with custom sound reminder saved to '{calendarMsgPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save calendar message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
