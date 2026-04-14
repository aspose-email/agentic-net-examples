using Aspose.Email.Calendar;
using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Configuration
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            string downloadFolder = Path.Combine(Environment.CurrentDirectory, "Attachments");

            // Ensure the download directory exists
            try
            {
                if (!Directory.Exists(downloadFolder))
                {
                    Directory.CreateDirectory(downloadFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare download folder: {ex.Message}");
                return;
            }

            // Create and connect the EWS client
            IEWSClient client;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client)
            {
                // Retrieve all appointments from the default calendar folder
                AppointmentCollection appointments;
                try
                {
                    appointments = client.ListAppointments();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list appointments: {ex.Message}");
                    return;
                }

                foreach (Appointment appointment in appointments)
                {
                    // Check if the appointment has attachments
                    if (appointment.Attachments != null && appointment.Attachments.Count > 0)
                    {
                        foreach (Attachment attachment in appointment.Attachments)
                        {
                            string filePath = Path.Combine(downloadFolder, attachment.Name ?? "UnnamedAttachment");

                            try
                            {
                                // Save the attachment content to the local file system
                                using (Stream contentStream = attachment.ContentStream)
                                using (FileStream fileStream = File.Create(filePath))
                                {
                                    contentStream.CopyTo(fileStream);
                                }

                                Console.WriteLine($"Saved attachment: {filePath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save attachment '{attachment.Name}': {ex.Message}");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
