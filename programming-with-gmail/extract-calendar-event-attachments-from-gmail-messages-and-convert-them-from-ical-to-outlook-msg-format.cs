using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

namespace GmailCalendarAttachmentExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – replace with real values or skip execution.
                string accessToken = "YOUR_ACCESS_TOKEN";
                string defaultEmail = "user@example.com";

                if (string.IsNullOrWhiteSpace(accessToken) || accessToken == "YOUR_ACCESS_TOKEN")
                {
                    Console.WriteLine("Access token is a placeholder. Skipping Gmail operations.");
                    return;
                }

                // Create Gmail client.
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

                // Ensure output directory exists.
                string outputDir = "ExtractedMsg";
                try
                {
                    if (!Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }

                // List all messages in the mailbox.
                System.Collections.Generic.List<GmailMessageInfo> messages = null;
                try
                {
                    messages = gmailClient.ListMessages();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list Gmail messages: {ex.Message}");
                    return;
                }

                foreach (GmailMessageInfo messageInfo in messages)
                {
                    MailMessage mailMessage = null;
                    try
                    {
                        mailMessage = gmailClient.FetchMessage(messageInfo.Id);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to fetch message {messageInfo.Id}: {ex.Message}");
                        continue;
                    }

                    // Process each attachment.
                    foreach (Attachment attachment in mailMessage.Attachments)
                    {
                        // Identify iCalendar attachments by extension or MIME type.
                        bool isIcs = false;
                        if (!string.IsNullOrEmpty(attachment.Name) && attachment.Name.EndsWith(".ics", StringComparison.OrdinalIgnoreCase))
                        {
                            isIcs = true;
                        }
                        else if (attachment.ContentType != null && attachment.ContentType.MediaType.Equals("text/calendar", StringComparison.OrdinalIgnoreCase))
                        {
                            isIcs = true;
                        }

                        if (!isIcs)
                            continue;

                        // Load the iCalendar content into an Appointment object.
                        Appointment appointment = null;
                        try
                        {
                            using (Stream icsStream = attachment.ContentStream)
                            {
                                appointment = Appointment.Load(icsStream);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to load iCalendar from attachment {attachment.Name}: {ex.Message}");
                            continue;
                        }

                        // Convert the Appointment to a MAPI message.
                        MapiMessage mapiMessage = null;
                        try
                        {
                            mapiMessage = appointment.ToMapiMessage();
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to convert appointment to MAPI message: {ex.Message}");
                            continue;
                        }

                        // Save the MAPI message as an Outlook MSG file.
                        string safeFileName = Path.GetFileNameWithoutExtension(attachment.Name);
                        if (string.IsNullOrWhiteSpace(safeFileName))
                        {
                            safeFileName = "CalendarEvent";
                        }
                        string msgPath = Path.Combine(outputDir, $"{safeFileName}.msg");
                        try
                        {
                            mapiMessage.Save(msgPath);
                            Console.WriteLine($"Saved MSG file: {msgPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save MSG file {msgPath}: {ex.Message}");
                        }
                    }

                    // Dispose the fetched mail message.
                    mailMessage.Dispose();
                }

                // Dispose the Gmail client if it implements IDisposable.
                if (gmailClient is IDisposable disposableClient)
                {
                    disposableClient.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
