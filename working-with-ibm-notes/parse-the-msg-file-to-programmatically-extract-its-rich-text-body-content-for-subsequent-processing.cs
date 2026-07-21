using Aspose.Email;
using Aspose.Email.Mapi;
using System;
using System.IO;

class Program
{
    // Author: Aspose.Email example – extracts rich‑text body from an Outlook MSG file.
    static void Main()
    {
        try
        {
            // Path to the MSG file.
            string msgPath = "sample.msg";

            // Verify that the input file exists.
            if (!File.Exists(msgPath))
            {
                // Create a placeholder MSG file if it does not exist.
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

            // Load the Outlook message.
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Extract the rich‑text (RTF) body. If unavailable, fall back to the plain‑text body.
                string rtfBody = msg.BodyRtf;
                if (string.IsNullOrEmpty(rtfBody))
                {
                    rtfBody = msg.Body;
                }

                // Output the extracted rich‑text content.
                Console.WriteLine("Rich‑Text Body:");
                Console.WriteLine(rtfBody);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
