using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;
using Aspose.Email.Calendar;

namespace AsposeEmailZimbraExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the Zimbra TGZ archive
                string tgzFilePath = "sample.tgz";

                // Verify TGZ file exists before attempting to read
                if (!File.Exists(tgzFilePath))
                {
                    Console.Error.WriteLine($"TGZ file not found: {tgzFilePath}");
                }
                else
                {
                    // Open the TGZ archive using TgzReader
                    using (TgzReader tgzReader = new TgzReader(tgzFilePath))
                    {
                        Console.WriteLine($"Current directory in TGZ: {tgzReader.CurrentDirectory}");

                        // Iterate through all messages in the archive
                        while (tgzReader.ReadNextMessage())
                        {
                            // CurrentMessage returns a MailMessage instance
                            MailMessage message = tgzReader.CurrentMessage;

                            // Display basic email information
                            Console.WriteLine("----- Email Message -----");
                            Console.WriteLine($"Subject: {message.Subject}");
                            Console.WriteLine($"From: {message.From}");
                            Console.WriteLine($"Date: {message.Date}");
                        }

                        // Export all messages and directory structure to a local folder
                        string exportFolder = "ExportedMessages";
                        try
                        {
                            if (!Directory.Exists(exportFolder))
                            {
                                Directory.CreateDirectory(exportFolder);
                            }
                            tgzReader.ExportTo(exportFolder);
                            Console.WriteLine($"Messages exported to: {exportFolder}");
                        }
                        catch (Exception exportEx)
                        {
                            Console.Error.WriteLine($"Error exporting TGZ contents: {exportEx.Message}");
                        }
                    }
                }

                // Path to an iCalendar (.ics) file representing a calendar entry
                string icsFilePath = "calendar.ics";

                // Verify the .ics file exists before loading
                if (!File.Exists(icsFilePath))
                {
                    Console.Error.WriteLine($"iCalendar file not found: {icsFilePath}");
                }
                else
                {
                    // Load the appointment from the .ics file
                    Appointment appointment = Appointment.Load(icsFilePath);

                    // Display calendar entry details
                    Console.WriteLine("----- Calendar Entry -----");
                    Console.WriteLine($"Summary: {appointment.Summary}");
                    Console.WriteLine($"Description: {appointment.Description}");
                    Console.WriteLine($"Location: {appointment.Location}");
                    Console.WriteLine($"Start: {appointment.StartDate}");
                    Console.WriteLine($"End: {appointment.EndDate}");
                }
            }
            catch (Exception ex)
            {
                // Top-level exception handling
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}