using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define file paths
            string emlPath = "appointment.eml";
            string msgPath = "appointment.msg";

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(emlPath));
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create attendees list
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@example.com"));
            attendees.Add(new MailAddress("person2@example.com"));

            // Create appointment with custom HTML body
            Appointment appointment = new Appointment(
                "Room 101",
                new DateTime(2023, 12, 25, 10, 0, 0),
                new DateTime(2023, 12, 25, 11, 0, 0),
                new MailAddress("organizer@example.com"),
                attendees)
            {
                Summary = "Christmas Meeting",
                HtmlDescription = "<html><body><h1>Agenda</h1><p>Discuss holiday plans.</p></body></html>"
            };

            // Convert appointment to MailMessage and save as EML
            using (MailMessage emlMessage = appointment.ToMailMessage())
            {
                emlMessage.Save(emlPath);
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

            // Load the EML, convert to MapiMessage, and save as MSG preserving HTML
            using (MailMessage loadedMessage = MailMessage.Load(emlPath))
            {
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(loadedMessage, MapiConversionOptions.UnicodeFormat))
                {
                    MsgSaveOptions msgOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                    mapiMessage.Save(msgPath, msgOptions);
                }
            }

            Console.WriteLine("Appointment saved as EML and converted to MSG successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
