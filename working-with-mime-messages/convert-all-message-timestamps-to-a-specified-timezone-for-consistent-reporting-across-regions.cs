using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output file paths
            string inputPath = "input.eml";
            string outputPath = "output.eml";

            // Ensure the input file exists; create a minimal placeholder if missing
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

                using (MailMessage placeholder = new MailMessage())
                {
                    placeholder.From = "sender@example.com";
                    placeholder.To = "recipient@example.com";
                    placeholder.Subject = "Placeholder";
                    placeholder.Body = "This is a placeholder email.";
                    placeholder.Save(inputPath);
                }
                Console.WriteLine($"Placeholder input file created at {inputPath}");
            }

            // Load the email message
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Specify the target timezone (e.g., Eastern Standard Time)
                TimeZoneInfo targetTimeZone;
                try
                {
                    targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                }
                catch (TimeZoneNotFoundException)
                {
                    Console.Error.WriteLine("Target timezone not found on this system.");
                    return;
                }
                catch (InvalidTimeZoneException)
                {
                    Console.Error.WriteLine("Target timezone data is invalid.");
                    return;
                }

                // Set the TimeZoneOffset to the target timezone's offset from UTC
                TimeSpan utcOffset = targetTimeZone.GetUtcOffset(DateTime.UtcNow);
                message.TimeZoneOffset = utcOffset;

                // Save the updated message
                message.Save(outputPath);
                Console.WriteLine($"Message saved with timezone offset {utcOffset} to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
