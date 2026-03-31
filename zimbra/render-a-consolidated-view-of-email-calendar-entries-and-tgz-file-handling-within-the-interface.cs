using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare a minimal EML file if it does not exist
            string emlPath = "sample.eml";
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

                try
                {
                    using (StreamWriter writer = new StreamWriter(emlPath, false))
                    {
                        writer.WriteLine("From: sender@example.com");
                        writer.WriteLine("To: recipient@example.com");
                        writer.WriteLine("Subject: Test Email");
                        writer.WriteLine();
                        writer.WriteLine("This is a test email generated as a placeholder.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the email message
            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(emlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load EML file: {ex.Message}");
                return;
            }

            // Placeholder Gmail credentials (do not perform real network calls)
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            // If placeholder credentials are detected, skip Gmail operations
            if (accessToken == "YOUR_ACCESS_TOKEN")
            {
                Console.WriteLine("Placeholder Gmail credentials detected. Skipping Gmail operations.");
            }
            else
            {
                // Initialize Gmail client
                IGmailClient gmailClient = null;
                try
                {
                    gmailClient = GmailClient.GetInstance(accessToken, defaultEmail);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                    return;
                }

                using (gmailClient as IDisposable)
                {
                    // List calendars
                    try
                    {
                        var calendars = gmailClient.ListCalendars();
                        Console.WriteLine("Calendars:");
                        foreach (var cal in calendars)
                        {
                            Console.WriteLine($"- Id: {cal.Id}, Summary: {cal.Summary}");
                            // List appointments for each calendar
                            try
                            {
                                var appointments = gmailClient.ListAppointments(cal.Id);
                                foreach (var appt in appointments)
                                {
                                    Console.WriteLine($"  * Appointment: {appt.Summary} ({appt.StartDate} - {appt.EndDate})");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to list appointments for calendar {cal.Id}: {ex.Message}");
                            }

                            // Create a new appointment and import it
                            try
                            {
                                var attendees = new MailAddressCollection { new MailAddress(defaultEmail) };
                                var newAppt = new Appointment(
                                    "New Meeting",
                                    DateTime.Now.AddHours(1),
                                    DateTime.Now.AddHours(2),
                                    new MailAddress(defaultEmail),
                                    attendees);
                                newAppt.Summary = "Automated Meeting";
                                newAppt.Description = "Created via Aspose.Email sample.";
                                gmailClient.ImportAppointment(cal.Id, newAppt);
                                Console.WriteLine($"  -> Imported new appointment into calendar {cal.Id}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to import appointment into calendar {cal.Id}: {ex.Message}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to list calendars: {ex.Message}");
                    }

                    // Send the loaded email message
                    try
                    {
                        string sentMessageId = gmailClient.SendMessage(mailMessage);
                        Console.WriteLine($"Email sent successfully. Message Id: {sentMessageId}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                    }
                }
            }

            // Create a placeholder TGZ archive containing the EML file
            string tgzPath = "archive.tgz";
            if (!File.Exists(tgzPath))
            {
                try
                {
                    // Simple placeholder: write the raw EML bytes into the TGZ file.
                    // In a real scenario, proper tar+gzip packaging would be used.
                    byte[] emlBytes = File.ReadAllBytes(emlPath);
                    using (FileStream tgzStream = new FileStream(tgzPath, FileMode.Create, FileAccess.Write))
                    {
                        tgzStream.Write(emlBytes, 0, emlBytes.Length);
                    }
                    Console.WriteLine($"Placeholder TGZ archive created at {tgzPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create TGZ archive: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"TGZ archive already exists at {tgzPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
