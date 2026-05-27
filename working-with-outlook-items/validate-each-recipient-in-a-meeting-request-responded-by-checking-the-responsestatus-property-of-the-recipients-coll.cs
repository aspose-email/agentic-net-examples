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
            string messagePath = "meetingRequest.msg";

            // Guard file existence
            if (!File.Exists(messagePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(messagePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"File not found: {messagePath}");
                return;
            }

            // Load the meeting request message
            using (MapiMessage meetingMessage = MapiMessage.Load(messagePath))
            {
                // Iterate over each recipient and output their response status
                foreach (MapiRecipient recipient in meetingMessage.Recipients)
                {
                    string email = recipient.EmailAddress ?? "(no address)";
                    MapiRecipientTrackStatus status = recipient.RecipientTrackStatus;
                    Console.WriteLine($"Recipient: {email}, Response Status: {status}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
