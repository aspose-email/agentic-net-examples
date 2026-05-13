using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string icsPath = "meeting.ics";
            string msgPath = "meeting.msg";

            // Ensure the input .ics file exists; create a minimal placeholder if missing
            if (!File.Exists(icsPath))
            {
                try
                {
                    DateTime start = DateTime.Now.AddHours(1);
                    DateTime end = start.AddHours(1);
                    MailAddressCollection attendees = new MailAddressCollection();
                    attendees.Add(new MailAddress("attendee@example.com"));
                    Appointment placeholder = new Appointment(
                        "Placeholder Meeting",
                        start,
                        end,
                        new MailAddress("organizer@example.com"),
                        attendees);
                    placeholder.Save(icsPath, AppointmentSaveFormat.Ics);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder .ics file: {ex.Message}");
                    return;
                }
            }

            // Load the appointment from the .ics file
            Appointment appointment;
            try
            {
                appointment = Appointment.Load(icsPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load .ics file: {ex.Message}");
                return;
            }

            DateTime originalStart = appointment.StartDate;

            // Convert the appointment to a MAPI message and save as .msg
            try
            {
                using (MapiMessage mapiMessage = appointment.ToMapiMessage())
                {
                    mapiMessage.Save(msgPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert and save .msg file: {ex.Message}");
                return;
            }

            // Verify that the saved .msg contains the correct start time
            try
            {
                MapiMessage loadedMsg = MapiMessage.Load(msgPath);
                if (loadedMsg.SupportedType == MapiItemType.Calendar)
                {
                    MapiCalendar calendar = (MapiCalendar)loadedMsg.ToMapiMessageItem();
                    DateTime msgStart = calendar.StartDate;

                    if (msgStart == originalStart)
                    {
                        Console.WriteLine("Validation succeeded: Start times match.");
                    }
                    else
                    {
                        Console.WriteLine($"Validation failed: Start time mismatch. Expected {originalStart}, found {msgStart}.");
                    }
                }
                else
                {
                    Console.WriteLine("The generated MSG file does not contain a calendar item.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load or validate .msg file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
