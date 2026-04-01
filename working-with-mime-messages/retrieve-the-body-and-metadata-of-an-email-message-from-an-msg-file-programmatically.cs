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
            string msgFilePath = "sample.msg";

            // Ensure the input MSG file exists; create a minimal placeholder if missing.
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                try
                {
                    // Create a simple placeholder MSG.
                    MapiMessage placeholderMessage = new MapiMessage(
                        "Placeholder Subject",
                        "This is a placeholder body.",
                        "sender@example.com",
                        "recipient@example.com");
                    placeholderMessage.Save(msgFilePath);
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {createEx.Message}");
                    return;
                }
            }

            // Load the MSG file.
            using (MapiMessage msg = MapiMessage.Load(msgFilePath))
            {
                // Output basic metadata.
                Console.WriteLine($"Subject: {msg.Subject}");
                Console.WriteLine($"Sender Email: {msg.SenderEmailAddress}");
                Console.WriteLine($"Sent Representing Email: {msg.SentRepresentingEmailAddress}");
                Console.WriteLine($"Sent Date: {msg.DeliveryTime}");
                Console.WriteLine($"Body (plain text): {msg.Body}");
                Console.WriteLine($"Body (HTML): {msg.BodyHtml}");

                // List attachments, if any.
                if (msg.Attachments != null && msg.Attachments.Count > 0)
                {
                    Console.WriteLine("Attachments:");
                    foreach (MapiAttachment attachment in msg.Attachments)
                    {
                        Console.WriteLine($"- {attachment.FileName}");
                    }
                }
                else
                {
                    Console.WriteLine("No attachments found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
