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
            // Path to the email file (MSG format)
            string emailPath = "email.msg";

            // Ensure the file exists; if not, create a minimal placeholder MSG
            if (!File.Exists(emailPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "This is a placeholder email body used when the expected file is missing."))
                    {
                        placeholder.Save(emailPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder email: {ex.Message}");
                    return;
                }
            }

            // Load the email and generate a summary
            try
            {
                using (MapiMessage message = MapiMessage.Load(emailPath))
                {
                    string subject = message.Subject ?? "(No Subject)";
                    string sender = message.SenderName ?? message.SenderEmailAddress ?? "(Unknown Sender)";
                    string body = message.Body ?? string.Empty;
                    string snippet = body.Length > 200 ? body.Substring(0, 200) : body;

                    Console.WriteLine($"Subject: {subject}");
                    Console.WriteLine($"Sender: {sender}");
                    Console.WriteLine("Body preview (first 200 characters):");
                    Console.WriteLine(snippet);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading or processing the email: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
