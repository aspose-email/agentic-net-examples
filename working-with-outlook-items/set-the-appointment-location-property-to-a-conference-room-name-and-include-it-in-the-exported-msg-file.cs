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
            // Define output MSG file path
            string outputPath = "appointment.msg";

            // Ensure the directory for the output file exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Guard file existence (not strictly needed for write, but follows the rule)
            try
            {
                // Create attendees collection
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("person1@domain.com"));
                attendees.Add(new MailAddress("person2@domain.com"));
                attendees.Add(new MailAddress("person3@domain.com"));

                // Create the appointment with a conference room location
                Appointment appointment = new Appointment(
                    "Conference Room A",                         // location
                    new DateTime(2024, 5, 20, 10, 0, 0),        // start time
                    new DateTime(2024, 5, 20, 11, 0, 0),        // end time
                    new MailAddress("organizer@domain.com"),    // organizer
                    attendees);

                // Set additional properties
                appointment.Summary = "Project Kickoff";
                appointment.Description = "Discuss project goals and timelines.";

                // Convert to MAPI message and save as MSG
                using (MapiMessage mapiMessage = appointment.ToMapiMessage())
                {
                    // Save the MSG file
                    mapiMessage.Save(outputPath);
                }

                Console.WriteLine("Appointment saved to MSG file: " + outputPath);
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine("File operation failed: " + ioEx.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
