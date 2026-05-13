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
            // Define file paths
            string emlPath = "appointment.eml";
            string msgPath = "appointment.msg";

            // Create attendees list
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));

            // Create appointment with basic details
            DateTime start = new DateTime(2024, 12, 1, 10, 0, 0);
            DateTime end = new DateTime(2024, 12, 1, 11, 0, 0);
            Appointment appointment = new Appointment(
                "Team Meeting",
                start,
                end,
                new MailAddress("organizer@domain.com"),
                attendees);

            // Set custom HTML body
            appointment.HtmlDescription = "<html><body><h1>Agenda</h1><p>Discuss project milestones.</p></body></html>";
            appointment.Summary = "Team Meeting";
            appointment.Description = "Meeting to discuss project milestones.";

            // Convert appointment to MailMessage (EML)
            using (MailMessage mailMessage = appointment.ToMailMessage())
            {
                try
                {
                    mailMessage.Save(emlPath);
                    Console.WriteLine($"EML file saved to: {emlPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save EML file: {ex.Message}");
                    return;
                }
            }

            // Verify EML file exists before loading
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine("EML file was not created.");
                return;
            }

            // Load MailMessage from EML
            MailMessage loadedMessage;
            try
            {
                loadedMessage = MailMessage.Load(emlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load EML file: {ex.Message}");
                return;
            }

            // Convert loaded MailMessage to MapiMessage (MSG)
            using (loadedMessage)
            using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(loadedMessage))
            {
                try
                {
                    mapiMessage.Save(msgPath);
                    Console.WriteLine($"MSG file saved to: {msgPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
