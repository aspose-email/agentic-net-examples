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
            string msgPath = "meeting.msg";

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

            // Load the MSG file safely
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Check if the message is a meeting request
                // Meeting requests typically have MessageClass "IPM.Schedule.Meeting.Request"
                if (!string.Equals(msg.MessageClass, "IPM.Schedule.Meeting.Request", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("The provided MSG file is not a meeting request.");
                    return;
                }

                // Extract attendees from the Recipients collection
                MapiRecipientCollection recipients = msg.Recipients;
                if (recipients == null || recipients.Count == 0)
                {
                    Console.WriteLine("No attendees found in the meeting request.");
                    return;
                }

                Console.WriteLine("Attendee email addresses:");
                foreach (MapiRecipient recipient in recipients)
                {
                    // Some recipients may not have an email address; skip those
                    if (!string.IsNullOrEmpty(recipient.EmailAddress))
                    {
                        Console.WriteLine(recipient.EmailAddress);
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
