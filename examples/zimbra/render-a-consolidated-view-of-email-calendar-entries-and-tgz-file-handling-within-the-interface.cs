using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Storage.Zimbra;

namespace AsposeEmailZimbraSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Path to the Zimbra TGZ archive
                string tgzPath = "sample.tgz";

                // Verify that the TGZ file exists before attempting to read it
                if (!File.Exists(tgzPath))
                {
                    Console.Error.WriteLine($"File not found: {tgzPath}");
                    return;
                }

                // Open the TGZ archive using a TgzReader inside a using block to ensure disposal
                using (TgzReader tgzReader = new TgzReader(tgzPath))
                {
                    // Get total number of items in the archive
                    int totalItems = tgzReader.GetTotalItemsCount();
                    Console.WriteLine($"Total items in TGZ archive: {totalItems}");

                    // Iterate through each item in the archive
                    for (int i = 0; i < totalItems; i++)
                    {
                        // Read the next message from the archive
                        tgzReader.ReadNextMessage();

                        // Retrieve the current message
                        MailMessage message = tgzReader.CurrentMessage;
                        if (message == null)
                        {
                            continue;
                        }

                        // Display basic email information
                        Console.WriteLine("--------------------------------------------------");
                        Console.WriteLine($"Subject : {message.Subject}");
                        Console.WriteLine($"From    : {message.From}");
                        Console.WriteLine($"Date    : {message.Date}");

                        // Check for calendar attachments (iCalendar files)
                        foreach (Attachment attachment in message.Attachments)
                        {
                            // Identify iCalendar files by extension
                            if (attachment.Name != null && attachment.Name.EndsWith(".ics", StringComparison.OrdinalIgnoreCase))
                            {
                                // Load the attachment content into a stream
                                using (MemoryStream icsStream = new MemoryStream())
                                {
                                    attachment.ContentStream.CopyTo(icsStream);
                                    icsStream.Position = 0;

                                    // Parse the iCalendar data into an Appointment object
                                    Appointment appointment = Appointment.Load(icsStream);
                                    if (appointment != null)
                                    {
                                        Console.WriteLine("  Calendar Entry:");
                                        Console.WriteLine($"    Summary     : {appointment.Summary}");
                                        Console.WriteLine($"    Location    : {appointment.Location}");
                                        Console.WriteLine($"    Start       : {appointment.StartDate}");
                                        Console.WriteLine($"    End         : {appointment.EndDate}");
                                        Console.WriteLine($"    Description : {appointment.Description}");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected errors without crashing the application
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}