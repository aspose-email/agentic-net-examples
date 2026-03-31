using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "sample.msg";

            // Ensure the MSG file exists
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

                try
                {
                    // Create a minimal placeholder MSG file
                    using (MapiMessage placeholder = new MapiMessage("Placeholder Sender", "placeholder@example.com", "Placeholder Subject", "Placeholder body"))
                    {
                        placeholder.Save(msgPath);
                    }
                    Console.WriteLine($"Placeholder MSG file created at '{msgPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Convert to MailMessage to access From.Address
                using (MailMessage mail = msg.ToMailMessage(new MailConversionOptions()))
                {
                    if (mail.From != null && !string.IsNullOrEmpty(mail.From.Address))
                    {
                        Console.WriteLine($"Sender Email: {mail.From.Address}");
                    }
                    else
                    {
                        Console.WriteLine("Sender email address not available.");
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
