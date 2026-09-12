using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    // Author: Aspose.Email .NET example
    static void Main()
    {
        try
        {
            string msgPath = "input.msg";
            string icsPath = "output.ics";

            // Guard file I/O
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Error: MSG file not found at '{msgPath}'.");
                return;
            }

            // Load MSG safely
            using (MapiMessage mapMsg = MapiMessage.Load(msgPath))
            {
                // Convert to MailMessage (requires MailConversionOptions in Aspose.Email.Mapi)
                var conversionOptions = new MailConversionOptions();
                using (MailMessage mailMsg = mapMsg.ToMailMessage(conversionOptions))
                {
                    try
                    {
                        // Save as iCalendar; library infers format from extension
                        mailMsg.Save(icsPath);
                        Console.WriteLine($"Successfully saved iCalendar to '{icsPath}'.");
                    }
                    catch (Exception ex)
                    {
                        // If conversion fails (e.g., not a calendar item), create minimal placeholder
                        Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                        try
                        {
                            File.WriteAllText(icsPath, "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nEND:VCALENDAR");
                            Console.WriteLine($"Placeholder iCalendar created at '{icsPath}'.");
                        }
                        catch (Exception placeholderEx)
                        {
                            Console.Error.WriteLine($"Failed to create placeholder: {placeholderEx.Message}");
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
