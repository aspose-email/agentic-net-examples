using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

namespace AsposeEmailExample
{
    // Author: Aspose.Email .NET sample
    class Program
    {
        static void Main(string[] args)
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

                    Console.Error.WriteLine($"Message file not found: {msgPath}");
                    return;
                }

                // Load the Outlook MSG file
                MapiMessage msg = MapiMessage.Load(msgPath);

                // Access the HTML body of the message
                string htmlBody = msg.BodyHtml;

                // Output the HTML content
                Console.WriteLine("HTML Body:");
                Console.WriteLine(htmlBody);
            }
            catch (Exception ex)
            {
                // Gracefully handle any unexpected errors
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
