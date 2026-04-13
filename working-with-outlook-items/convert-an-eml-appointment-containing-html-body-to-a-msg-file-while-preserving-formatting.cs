using Aspose.Email.Calendar;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            const string emlPath = "appointment.eml";
            const string msgPath = "appointment.msg";

            // Ensure the input EML file exists; create a minimal placeholder if missing.
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (var fs = new FileStream(emlPath, FileMode.Create, FileAccess.Write))
                    using (var writer = new StreamWriter(fs))
                    {
                        writer.WriteLine("Subject: Sample Appointment");
                        writer.WriteLine("Content-Type: text/html; charset=utf-8");
                        writer.WriteLine();
                        writer.WriteLine("<html><body><p>This is a sample appointment with <b>HTML</b> body.</p></body></html>");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the EML message with options that preserve embedded content.
            MailMessage mailMessage;
            try
            {
                var emlLoadOptions = new EmlLoadOptions
                {
                    PreserveTnefAttachments = true,
                    PreserveEmbeddedMessageFormat = true
                };
                mailMessage = MailMessage.Load(emlPath, emlLoadOptions);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load EML file: {ex.Message}");
                return;
            }

            // Convert the MailMessage (which may contain an HTML body) to a MapiMessage.
            MapiMessage mapiMessage;
            try
            {
                // Use Unicode format to keep HTML body intact.
                var conversionOptions = MapiConversionOptions.UnicodeFormat;
                mapiMessage = MapiMessage.FromMailMessage(mailMessage, conversionOptions);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert to MAPI message: {ex.Message}");
                return;
            }

            // Save the resulting MSG file.
            try
            {
                mapiMessage.Save(msgPath);
                Console.WriteLine($"Successfully saved MSG file to '{msgPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
            }
            finally
            {
                // Dispose resources.
                mailMessage?.Dispose();
                mapiMessage?.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
