using System;
using System.IO;
using System.Text;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "message.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            try
            {
                using (MapiMessage msg = MapiMessage.Load(msgPath))
                {
                    string subject = msg.Subject ?? string.Empty;
                    string sender = msg.SenderName ?? msg.SenderEmailAddress ?? string.Empty;

                    StringBuilder recipientBuilder = new StringBuilder();
                    foreach (MapiRecipient recipient in msg.Recipients)
                    {
                        if (recipientBuilder.Length > 0)
                            recipientBuilder.Append("; ");

                        recipientBuilder.Append(recipient.EmailAddress ?? string.Empty);
                    }
                    string recipients = recipientBuilder.ToString();

                    Console.WriteLine($"Subject: {subject}");
                    Console.WriteLine($"Sender: {sender}");
                    Console.WriteLine($"Recipients: {recipients}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
