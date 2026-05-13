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
            // Path to the MSG file containing an RFC‑2047 encoded Subject header
            string msgPath = "sample.msg";

            // Guard against missing file
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

            // Load the Outlook message inside a using block to ensure disposal
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                // NormalizedSubject returns the decoded Unicode subject
                string decodedSubject = message.NormalizedSubject;

                Console.WriteLine("Decoded Subject: " + decodedSubject);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
