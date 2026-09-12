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
            // Path to the MSG file
            string msgPath = "sample.msg";

            // Verify that the file exists before attempting to load it
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

                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load the Outlook MSG file
            MapiMessage msg = MapiMessage.Load(msgPath);

            // Convert the MAPI message to a MailMessage to access Body and HtmlBody
            using (MailMessage mailMessage = msg.ToMailMessage(new MailConversionOptions()))
            {
                // Output plain‑text body
                Console.WriteLine("Plain Text Body:");
                Console.WriteLine(mailMessage.Body ?? string.Empty);
                Console.WriteLine();

                // Output HTML body
                Console.WriteLine("HTML Body:");
                Console.WriteLine(mailMessage.HtmlBody ?? string.Empty);
            }
        }
        catch (Exception ex)
        {
            // Gracefully report any unexpected errors
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
