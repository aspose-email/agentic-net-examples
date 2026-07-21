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
            // Input MSG file path
            string msgPath = "input.msg";
            // Output iCalendar file path
            string icsPath = "output.ics";

            // Ensure the input MSG file exists; if not, create a minimal placeholder
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

                // Create an empty MSG placeholder (zero‑byte file)
                File.WriteAllBytes(msgPath, new byte[0]);
                Console.Error.WriteLine($"Input file '{msgPath}' was missing and has been created as an empty placeholder.");
                return;
            }

            // Load the MSG message
            MapiMessage mapMsg = MapiMessage.Load(msgPath);

            // Convert to MailMessage (requires MailConversionOptions from Aspose.Email.Mapi)
            MailConversionOptions convOpts = new MailConversionOptions();
            using (MailMessage mailMsg = mapMsg.ToMailMessage(convOpts))
            {
                // Save as iCalendar (.ics). The library infers format from the extension.
                mailMsg.Save(icsPath);
                Console.WriteLine($"Successfully converted '{msgPath}' to '{icsPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
