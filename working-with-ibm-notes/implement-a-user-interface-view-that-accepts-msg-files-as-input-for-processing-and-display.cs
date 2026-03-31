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
            Console.Write("Enter the path to the MSG file: ");
            string msgPath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(msgPath))
            {
                Console.Error.WriteLine("Error: No path provided.");
                return;
            }

            // Ensure the directory exists
            string directory = Path.GetDirectoryName(msgPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create directory – {ex.Message}");
                    return;
                }
            }

            // Guard file existence; create a minimal placeholder if missing
            if (!File.Exists(msgPath))
            {
                try
                {
                    MailMessage placeholder = new MailMessage();
                    placeholder.Subject = "Placeholder Subject";
                    placeholder.Body = "This is a placeholder message.";
                    MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat);
                    placeholder.Save(msgPath, saveOptions);
                    Console.WriteLine($"Placeholder MSG created at: {msgPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create placeholder MSG – {ex.Message}");
                    return;
                }
            }

            // Load and display the MSG file
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                Console.WriteLine($"Subject: {message.Subject}");
                Console.WriteLine($"Sender Email: {message.SenderEmailAddress}");
                Console.WriteLine($"Body: {message.Body}");

                if (message.Attachments != null && message.Attachments.Count > 0)
                {
                    Console.WriteLine("Attachments:");
                    foreach (MapiAttachment attachment in message.Attachments)
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
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
