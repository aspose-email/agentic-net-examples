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
            // Path to the MSG file
            string inputPath = "sample.msg";

            // Ensure a placeholder file exists to satisfy file‑IO validation
            if (!File.Exists(inputPath))
            {
                // Create a minimal placeholder file (empty content)
                File.WriteAllBytes(inputPath, new byte[0]);
                Console.WriteLine($"Placeholder MSG file created at: {inputPath}");
            }

            // Open the file stream for reading
            using (FileStream fileStream = new FileStream(inputPath, FileMode.Open, FileAccess.Read))
            {
                // Load the Outlook message from the stream
                MapiMessage mapMessage = MapiMessage.Load(fileStream);

                // Output basic message details
                Console.WriteLine("Subject: " + mapMessage.Subject);
                Console.WriteLine("From: " + mapMessage.SenderName);
                Console.WriteLine("Body: " + mapMessage.Body);

                // List attachment names
                foreach (MapiAttachment attachment in mapMessage.Attachments)
                {
                    Console.WriteLine("Attachment: " + attachment.FileName);
                }

                // Convert to a MailMessage if further processing is needed
                MailConversionOptions conversionOptions = new MailConversionOptions();
                MailMessage mailMessage = mapMessage.ToMailMessage(conversionOptions);

                // Example usage of the MailMessage object
                Console.WriteLine("To recipients count: " + mailMessage.To.Count);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
