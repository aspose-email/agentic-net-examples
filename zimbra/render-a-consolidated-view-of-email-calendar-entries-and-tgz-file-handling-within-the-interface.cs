using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;
using Aspose.Email.Calendar;
using Aspose.Email.Mime;

namespace AsposeEmailZimbraExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string tgzFilePath = "sample.tgz";

                if (!File.Exists(tgzFilePath))
                {
                    Console.Error.WriteLine("The TGZ file '{0}' does not exist.", tgzFilePath);
                    return;
                }

                using (TgzReader tgzReader = new TgzReader(tgzFilePath))
                {
                    while (tgzReader.ReadNextMessage())
                    {
                        MailMessage currentMessage = tgzReader.CurrentMessage;
                        if (currentMessage == null)
                        {
                            continue;
                        }

                        Console.WriteLine("Subject: {0}", currentMessage.Subject);
                        Console.WriteLine("From: {0}", currentMessage.From);
                        Console.WriteLine("Date: {0}", currentMessage.Date);

                        // Process attachments that may contain calendar information
                        foreach (Attachment attachment in currentMessage.Attachments)
                        {
                            if (attachment == null)
                            {
                                continue;
                            }

                            string attachmentName = attachment.Name ?? string.Empty;
                            string mediaType = attachment.ContentType?.MediaType ?? string.Empty;

                            bool isCalendarAttachment = mediaType.Equals("text/calendar", StringComparison.OrdinalIgnoreCase) ||
                                                         attachmentName.EndsWith(".ics", StringComparison.OrdinalIgnoreCase);

                            if (isCalendarAttachment)
                            {
                                using (MemoryStream attachmentStream = new MemoryStream())
                                {
                                    attachment.ContentStream.CopyTo(attachmentStream);
                                    attachmentStream.Position = 0;

                                    try
                                    {
                                        Appointment appointment = Appointment.Load(attachmentStream);
                                        Console.WriteLine("  Calendar Entry:");
                                        Console.WriteLine("    Summary: {0}", appointment.Summary);
                                        Console.WriteLine("    Location: {0}", appointment.Location);
                                        Console.WriteLine("    Start: {0}", appointment.StartDate);
                                        Console.WriteLine("    End: {0}", appointment.EndDate);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.Error.WriteLine("    Failed to load calendar entry from attachment '{0}': {1}", attachmentName, ex.Message);
                                    }
                                }
                            }
                        }

                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("An error occurred: {0}", ex.Message);
            }
        }
    }
}