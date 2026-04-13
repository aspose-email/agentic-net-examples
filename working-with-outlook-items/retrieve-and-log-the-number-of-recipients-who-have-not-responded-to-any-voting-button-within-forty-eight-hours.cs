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
            string msgPath = "message.msg";

            // Ensure the MSG file exists; create a minimal placeholder if missing.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage())
                    {
                        placeholder.Subject = "Placeholder";
                        placeholder.Body = "This is a placeholder message.";
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the message.
            MapiMessage message;
            try
            {
                message = MapiMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            using (message)
            {
                // Retrieve follow‑up options (voting buttons, etc.).
                FollowUpOptions options = FollowUpManager.GetOptions(message);

                // If there are no voting buttons, nothing to count.
                if (string.IsNullOrEmpty(options?.VotingButtons))
                {
                    Console.WriteLine("No voting buttons defined on this message.");
                    return;
                }

                // Determine if 48 hours have passed since the message was sent.
                DateTime sentTime = message.ClientSubmitTime;
                bool elapsed48Hours = (DateTime.Now - sentTime) >= TimeSpan.FromHours(48);

                // Count recipients with no response (RecipientTrackStatus == None).
                int notRespondedCount = 0;
                foreach (MapiRecipient recipient in message.Recipients)
                {
                    if (recipient.RecipientTrackStatus == MapiRecipientTrackStatus.None)
                    {
                        // If the 48‑hour window has elapsed, count the recipient.
                        if (elapsed48Hours)
                        {
                            notRespondedCount++;
                        }
                    }
                }

                Console.WriteLine($"Recipients who have not responded within 48 hours: {notRespondedCount}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
