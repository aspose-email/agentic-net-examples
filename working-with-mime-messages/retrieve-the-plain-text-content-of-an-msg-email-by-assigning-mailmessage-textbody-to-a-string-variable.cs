using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

// Author: Aspose.Email example - retrieve plain‑text body from an MSG file
class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "sample.msg";

            // Verify the input file exists
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

                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Load the Outlook MSG file
            MapiMessage mapiMsg = MapiMessage.Load(msgPath);

            // Convert MapiMessage to MailMessage
            MailConversionOptions conversionOptions = new MailConversionOptions();
            using (MailMessage mailMessage = mapiMsg.ToMailMessage(conversionOptions))
            {
                // Retrieve the plain‑text body content
                string plainText = mailMessage.Body; // MailMessage.Body returns the plain‑text body
                Console.WriteLine("Plain‑text body:");
                Console.WriteLine(plainText);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
