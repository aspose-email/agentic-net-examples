using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Amp;

namespace AmpEmailParser
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the MSG file containing the AMP email
                string msgFilePath = "sample.msg";

                // Verify that the input file exists
                if (!File.Exists(msgFilePath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Input file not found: {msgFilePath}");
                    return;
                }

                // Load the Outlook MSG file
                MapiMessage mapiMessage = MapiMessage.Load(msgFilePath);

                // Convert the MAPI message to a MailMessage using default conversion options
                MailConversionOptions conversionOptions = new MailConversionOptions();
                using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
                {
                    // Attempt to treat the message as an AMP message
                    AmpMessage ampMessage = mailMessage as AmpMessage;

                    if (ampMessage != null)
                    {
                        // Access AMP structured content (example: list AMP components)
                        // The AmpMessage class provides methods to work with AMP components.
                        // Here we simply indicate that AMP content is present.
                        Console.WriteLine("The message contains AMP content.");
                    }
                    else
                    {
                        Console.WriteLine("The message does not contain AMP content.");
                    }

                    // Access common fields
                    Console.WriteLine($"Subject: {mailMessage.Subject}");
                    Console.WriteLine($"From: {mailMessage.From}");
                    Console.WriteLine($"Body (Text): {mailMessage.Body}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
