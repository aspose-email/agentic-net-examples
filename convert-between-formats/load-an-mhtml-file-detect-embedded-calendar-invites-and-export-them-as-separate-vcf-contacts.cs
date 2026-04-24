using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string mhtmlPath = "input.mhtml";
            string outputFolder = "Contacts";

            // Guard input file existence
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

                Console.Error.WriteLine($"Input file not found: {mhtmlPath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                Directory.CreateDirectory(outputFolder);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Load MHTML message
            MailMessage message;
            try
            {
                message = MailMessage.Load(mhtmlPath, new MhtmlLoadOptions());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MHTML file: {ex.Message}");
                return;
            }

            using (message)
            {
                List<MapiContact> contacts = new List<MapiContact>();

                foreach (Attachment attachment in message.Attachments)
                {
                    // Identify calendar attachments
                    bool isCalendar = string.Equals(attachment.ContentType.MediaType, "text/calendar", StringComparison.OrdinalIgnoreCase) ||
                                      Path.GetExtension(attachment.Name).Equals(".ics", StringComparison.OrdinalIgnoreCase);

                    if (!isCalendar)
                        continue;

                    // Load appointment from attachment stream
                    Appointment appointment;
                    try
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            attachment.ContentStream.CopyTo(ms);
                            ms.Position = 0;
                            appointment = Appointment.Load(ms);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to load appointment from attachment '{attachment.Name}': {ex.Message}");
                        continue;
                    }

                    // Extract attendees as contacts
                    foreach (MailAddress attendee in appointment.Attendees)
                    {
                        string displayName = string.IsNullOrEmpty(attendee.DisplayName) ? attendee.Address : attendee.DisplayName;
                        MapiContact contact = new MapiContact(displayName, attendee.Address);
                        contacts.Add(contact);
                    }
                }

                // Save each contact as VCF
                foreach (MapiContact contact in contacts)
                {
                    string safeFileName = contact.ElectronicAddresses.Email1?.EmailAddress ?? "contact";
                    string vcfPath = Path.Combine(outputFolder, $"{safeFileName}.vcf");

                    try
                    {
                        contact.Save(vcfPath);
                        Console.WriteLine($"Saved contact: {vcfPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save contact '{safeFileName}': {ex.Message}");
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
