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
            string msgPath = "appointment.msg";

            // Verify the input MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Load the existing appointment from the MSG file
            Appointment appointment;
            try
            {
                appointment = Appointment.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading appointment: {ex.Message}");
                return;
            }

            // Update appointment details
            appointment.Summary = "Updated Project Meeting";
            appointment.Location = "Conference Room B";
            appointment.StartDate = new DateTime(2023, 12, 1, 10, 0, 0);
            appointment.EndDate = new DateTime(2023, 12, 1, 11, 0, 0);

            // Update attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("alice@example.com"));
            attendees.Add(new MailAddress("bob@example.com"));
            appointment.Attendees = attendees;

            // Convert back to MAPI message and save over the original MSG file
            using (MapiMessage mapiMessage = appointment.ToMapiMessage())
            {
                try
                {
                    mapiMessage.Save(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving updated MSG: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
