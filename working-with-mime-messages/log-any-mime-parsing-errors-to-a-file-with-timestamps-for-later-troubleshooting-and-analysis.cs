using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        // Paths for the EML file to validate and the log file.
        string emlPath = "sample.eml";
        string logPath = "mime_errors.log";

        // Ensure the EML file exists; create a minimal placeholder if it does not.
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
                using (var placeholderWriter = new StreamWriter(emlPath, false))
                {
                    placeholderWriter.WriteLine("Subject: Placeholder");
                    placeholderWriter.WriteLine();
                    placeholderWriter.WriteLine("This is a minimal placeholder EML file.");
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Failed to create placeholder EML file: {ioEx.Message}");
                return;
            }
        }

        try
        {
            // Attempt to load (parse) the MIME message.
            var message = MailMessage.Load(emlPath);
            Console.WriteLine("No MIME parsing errors were found.");
        }
        catch (Exception parseEx)
        {
            // Log parsing errors with timestamps.
            try
            {
                using (var logWriter = new StreamWriter(logPath, true))
                {
                    logWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {parseEx.Message}");
                }
                Console.Error.WriteLine($"MIME parsing error logged to {logPath}");
            }
            catch (Exception logEx)
            {
                Console.Error.WriteLine($"Failed to write log file: {logEx.Message}");
            }
        }
    }
}
