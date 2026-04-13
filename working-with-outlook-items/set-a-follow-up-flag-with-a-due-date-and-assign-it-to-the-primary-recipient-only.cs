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
            // Define output MSG file path
            string msgPath = "FollowUpMessage.msg";

            // Ensure the directory exists if a path is provided
            string directory = Path.GetDirectoryName(msgPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a new MAPI message in draft mode
            using (MapiMessage message = new MapiMessage())
            {
                // Set primary recipient
                message.Recipients.Add("primary.recipient@example.com", "Primary Recipient", MapiRecipientType.MAPI_TO);

                // Set subject and body
                message.Subject = "Action Required: Follow‑up Needed";
                message.Body = "Please review the attached information and respond by the due date.";

                // Set follow‑up flag with start date (now) and due date (2 days later)
                DateTime startDate = DateTime.Now;
                DateTime dueDate = startDate.AddDays(2);
                FollowUpManager.SetFlag(message, "Please follow up", startDate, dueDate);

                // Save the message to disk
                try
                {
                    message.Save(msgPath);
                    Console.WriteLine($"Message saved to '{msgPath}'.");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to save message: {ioEx.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
