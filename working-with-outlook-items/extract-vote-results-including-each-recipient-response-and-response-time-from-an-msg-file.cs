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
            string msgPath = "sample.msg";

            // Guard file existence
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

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                Console.WriteLine($"Subject: {msg.Subject}");
                Console.WriteLine("Vote results:");

                // Iterate over recipients and output response status
                foreach (MapiRecipient recipient in msg.Recipients)
                {
                    // Use MAPI_TO recipient type as required
                    if (recipient.RecipientType == MapiRecipientType.MAPI_TO)
                    {
                        // Recipient response status (e.g., Accepted, Declined, etc.)
                        MapiRecipientTrackStatus status = recipient.RecipientTrackStatus;

                        // Output recipient email and status
                        Console.WriteLine($"Recipient: {recipient.EmailAddress}, Status: {status}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
