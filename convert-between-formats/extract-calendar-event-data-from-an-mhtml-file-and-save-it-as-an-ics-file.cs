using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: This example loads an MHTML email containing a calendar event and saves it as an iCalendar (ICS) file.
            string inputPath = "event.mhtml";
            string outputPath = "event.ics";

            // Verify input file exists
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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the MHTML message
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Save the message as an iCalendar file; the .ics extension infers the correct format
                message.Save(outputPath);
                Console.WriteLine($"Calendar event saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
