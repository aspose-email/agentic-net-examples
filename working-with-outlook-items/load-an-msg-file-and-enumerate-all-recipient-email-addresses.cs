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

            // Load the MSG file inside a using block to ensure proper disposal
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Enumerate all recipients and output their email addresses
                foreach (MapiRecipient recipient in msg.Recipients)
                {
                    // Optionally filter only primary (To) recipients
                    if (recipient.RecipientType == MapiRecipientType.MAPI_TO)
                    {
                        Console.WriteLine(recipient.EmailAddress);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Gracefully handle any unexpected errors
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
