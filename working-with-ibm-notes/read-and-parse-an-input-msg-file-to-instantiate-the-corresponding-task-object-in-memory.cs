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
            string msgPath = "input.msg";

            // Ensure the input MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(msgPath))
            {
                try
                {
                    // Create a simple placeholder message.
                    using (MapiMessage placeholder = new MapiMessage(
                        "sender@example.com",
                        "receiver@example.com",
                        "Placeholder Subject",
                        "This is a placeholder message body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file into a MapiMessage instance.
            try
            {
                using (MapiMessage message = MapiMessage.Load(msgPath))
                {
                    // Example: output some basic properties.
                    Console.WriteLine($"Subject: {message.Subject}");
                    Console.WriteLine($"Sender: {message.SenderName} <{message.SenderEmailAddress}>");
                    Console.WriteLine($"Body: {message.Body}");
                    Console.WriteLine($"Number of Attachments: {message.Attachments.Count}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
