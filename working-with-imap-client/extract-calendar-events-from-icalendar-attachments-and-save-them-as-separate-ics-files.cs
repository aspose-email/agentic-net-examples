using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the source email file containing iCalendar attachments
            string emailPath = "message.eml";

            // Verify the email file exists before attempting to load
            if (!File.Exists(emailPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emailPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Email file not found: {emailPath}");
                return;
            }

            // Load the email message
            using (MailMessage message = MailMessage.Load(emailPath))
            {
                // Ensure the output directory exists
                string outputDir = "output";
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Iterate over attachments and process iCalendar files
                for (int i = 0; i < message.Attachments.Count; i++)
                {
                    Attachment attachment = message.Attachments[i];
                    using (attachment)
                    {
                        // Identify iCalendar attachments by file extension
                        if (attachment.Name != null && attachment.Name.EndsWith(".ics", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                // Load the appointment from the attachment stream
                                using (Stream attStream = attachment.ContentStream)
                                {
                                    Appointment appointment = Appointment.Load(attStream);

                                    // Build a unique output file path for the extracted .ics file
                                    string outputPath = Path.Combine(outputDir, attachment.Name);

                                    // Save the appointment as an iCalendar file
                                    appointment.Save(outputPath);
                                    Console.WriteLine($"Extracted calendar saved to: {outputPath}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to process attachment '{attachment.Name}': {ex.Message}");
                                // Continue with next attachment
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
