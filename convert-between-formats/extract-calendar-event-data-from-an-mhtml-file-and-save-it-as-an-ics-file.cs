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
            // Paths for input MHTML and output ICS files
            string mhtmlPath = "input.mht";
            string icsPath = "output.ics";

            // Verify that the input file exists
            if (!File.Exists(mhtmlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(mhtmlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file '{mhtmlPath}' not found.");
                return;
            }

            // Load the MHTML message
            using (MailMessage message = MailMessage.Load(mhtmlPath))
            {
                // Locate a calendar attachment (text/calendar or .ics file)
                Attachment calendarAttachment = null;
                foreach (Attachment att in message.Attachments)
                {
                    if (att.ContentType.MediaType.Equals("text/calendar", StringComparison.OrdinalIgnoreCase) ||
                        (att.Name != null && att.Name.EndsWith(".ics", StringComparison.OrdinalIgnoreCase)))
                    {
                        calendarAttachment = att;
                        break;
                    }
                }

                if (calendarAttachment == null)
                {
                    Console.Error.WriteLine("No calendar attachment found in the MHTML file.");
                    return;
                }

                // Extract the attachment content and load it as an Appointment
                using (MemoryStream calStream = new MemoryStream())
                {
                    calendarAttachment.ContentStream.CopyTo(calStream);
                    calStream.Position = 0;

                    Appointment appointment = Appointment.Load(calStream);
                    // Save the appointment as an iCalendar (.ics) file
                    appointment.Save(icsPath);
                    Console.WriteLine($"Calendar saved to '{icsPath}'.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
