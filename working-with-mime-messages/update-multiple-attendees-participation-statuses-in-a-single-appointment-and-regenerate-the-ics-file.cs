using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            string icsPath = "appointment.ics";

            // Ensure the .ics file exists; create a minimal placeholder if missing
            if (!File.Exists(icsPath))
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(icsPath, false))
                    {
                        writer.WriteLine("BEGIN:VCALENDAR");
                        writer.WriteLine("VERSION:2.0");
                        writer.WriteLine("PRODID:-//Aspose//EN");
                        writer.WriteLine("BEGIN:VEVENT");
                        writer.WriteLine("UID:placeholder-uid");
                        writer.WriteLine("DTSTAMP:20230101T000000Z");
                        writer.WriteLine("DTSTART:20230102T100000Z");
                        writer.WriteLine("DTEND:20230102T110000Z");
                        writer.WriteLine("SUMMARY:Placeholder Event");
                        writer.WriteLine("END:VEVENT");
                        writer.WriteLine("END:VCALENDAR");
                    }
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
                Console.Error.WriteLine($"Failed to load appointment: {ex.Message}");
                return;
            }

            // Update participation status for all attendees
            try
            {
                foreach (dynamic attendee in appointment.Attendees)
                {
                    attendee.ParticipationStatus = ParticipationStatus.Accepted;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to update attendees: {ex.Message}");
                return;
            }

            // Save the updated appointment back to the .ics file
            try
            {
                appointment.Save(icsPath);
                Console.WriteLine("Appointment updated and saved successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save updated appointment: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
