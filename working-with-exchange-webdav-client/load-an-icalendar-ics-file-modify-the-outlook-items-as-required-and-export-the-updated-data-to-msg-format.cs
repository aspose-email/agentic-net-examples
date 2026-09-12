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
            // Input and output file paths
            string icsPath = "input.ics";
            string msgPath = "output.msg";

            // Guard against missing input file
            if (!File.Exists(icsPath))
            {
                Console.Error.WriteLine($"Input file not found: {icsPath}");
                return;
            }

            // Load the iCalendar file into an Appointment object
            Appointment appointment = Appointment.Load(icsPath);

            // Example modification: change the subject (Summary) of the appointment
            appointment.Summary = "Updated Subject";

            // Convert the modified appointment to a MAPI message
            MapiMessage mapMessage = appointment.ToMapiMessage();

            // Save the MAPI message as an Outlook MSG file
            mapMessage.Save(msgPath);

            Console.WriteLine($"Successfully saved MSG file to: {msgPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
