using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to a sample MSG file
            string msgPath = "sample.msg";

            // Ensure the sample MSG file exists; create a minimal placeholder if missing
            try
            {
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

                    using (MapiMessage placeholder = new MapiMessage("Placeholder Subject", "Placeholder Body", "sender@example.com", "receiver@example.com"))
                    {
                        placeholder.Save(msgPath);
                    }
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"File I/O error: {ioEx.Message}");
                return;
            }

            // List of supported MSG handling capabilities
            string[] capabilities = new string[]
            {
                "Load from file",
                "Load from stream",
                "Load with LoadOptions",
                "Save to file",
                "Save to stream",
                "Save with SaveOptions",
                "Check if stream is MSG format",
                "Check if file is MSG format",
                "Destroy attachments in MSG file",
                "Remove attachments from MSG file",
                "Convert from MailMessage",
                "Convert to MailMessage",
                "Access Body, HtmlBody, BodyRtf",
                "Access Attachments, Recipients, NamedProperties",
                "Set custom properties",
                "Encrypt / Decrypt",
                "Check signature",
                "Check bounced status",
                "Clone message",
                "Set message flags"
            };

            Console.WriteLine("Supported MSG capabilities:");
            foreach (string capability in capabilities)
            {
                Console.WriteLine("- " + capability);
            }

            // Demonstrate loading an MSG file
            try
            {
                using (MapiMessage msg = MapiMessage.Load(msgPath))
                {
                    Console.WriteLine($"Loaded MSG: Subject = {msg.Subject}");

                    // Demonstrate saving the message to a new file
                    string outputPath = "output.msg";
                    msg.Save(outputPath);
                    Console.WriteLine($"Message saved to {outputPath}");
                }
            }
            catch (Exception msgEx)
            {
                Console.Error.WriteLine($"MSG processing error: {msgEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
