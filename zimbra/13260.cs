using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;
using Aspose.Email.Calendar;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the Zimbra TGZ archive (adjust as needed)
            string tgzPath = "sample.tgz";

            // Verify that the TGZ file exists before attempting to read it
            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"File not found: {tgzPath}");
                return;
            }

            // Open the TGZ archive using the Aspose.Email TgzReader
            using (TgzReader tgzReader = new TgzReader(tgzPath))
            {
                Console.WriteLine($"Reading TGZ archive: {tgzPath}");

                // Iterate through all messages in the archive
                while (tgzReader.ReadNextMessage())
                {
                    // CurrentMessage returns a MailMessage instance
                    using (MailMessage message = tgzReader.CurrentMessage)
                    {
                        Console.WriteLine("----- Message -----");
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"Date: {message.Date}");

                        // Check for calendar attachments (iCalendar files)
                        bool calendarFound = false;
                        foreach (Attachment attachment in message.Attachments)
                        {
                            if (attachment.ContentType.MediaType.Equals("text/calendar", StringComparison.OrdinalIgnoreCase))
                            {
                                calendarFound = true;
                                // Load the calendar data from the attachment stream
                                using (MemoryStream calendarStream = new MemoryStream())
                                {
                                    attachment.ContentStream.CopyTo(calendarStream);
                                    calendarStream.Position = 0;
                                    Appointment appointment = Appointment.Load(calendarStream);
                                    Console.WriteLine("  Calendar Appointment:");
                                    Console.WriteLine($"    Summary: {appointment.Summary}");
                                    Console.WriteLine($"    Start: {appointment.StartDate}");
                                    Console.WriteLine($"    End: {appointment.EndDate}");
                                }
                                break;
                            }
                        }

                        if (!calendarFound)
                        {
                            Console.WriteLine("  No calendar attachment found.");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Output any unexpected errors without crashing the application
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}