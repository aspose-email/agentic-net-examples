using Aspose.Email.Calendar;
using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "appointment.eml";
            string outputPath = "appointment.msg";

            // Ensure input file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    MailMessage placeholder = new MailMessage();
                    placeholder.From = new MailAddress("organizer@example.com");
                    placeholder.To.Add(new MailAddress("attendee@example.com"));
                    placeholder.Subject = "Sample Appointment";
                    placeholder.HtmlBody = "<html><body><h1>Meeting</h1><p>Details of the appointment.</p></body></html>";

                    // Save placeholder EML.
                    placeholder.Save(inputPath, SaveOptions.DefaultEml);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists.
            try
            {
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Load the EML with options to preserve content.
            try
            {
                EmlLoadOptions emlLoadOptions = new EmlLoadOptions()
                {
                    PreserveTnefAttachments = true,
                    PreserveEmbeddedMessageFormat = true
                };

                using (MailMessage message = MailMessage.Load(inputPath, emlLoadOptions))
                {
                    // Save as MSG preserving original dates and formatting.
                    MsgSaveOptions msgSaveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                    {
                        PreserveOriginalDates = true
                    };

                    message.Save(outputPath, msgSaveOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                return;
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
