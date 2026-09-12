using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the TNEF file (e.g., winmail.dat)
            string tnefPath = "sample.tnef";

            // Verify that the TNEF file exists
            if (!File.Exists(tnefPath))
            {
                Console.Error.WriteLine($"TNEF file not found: {tnefPath}");
                return;
            }

            // Load the TNEF message
            using (MapiMessage tnefMessage = MapiMessage.LoadFromTnef(tnefPath))
            {
                // Display basic properties
                Console.WriteLine("Subject: " + tnefMessage.Subject);
                Console.WriteLine("From: " + tnefMessage.SenderName);
                Console.WriteLine("Body: " + tnefMessage.Body);

                // Directory to save attachments
                string attachmentsDir = Path.Combine(Directory.GetCurrentDirectory(), "Attachments");
                Directory.CreateDirectory(attachmentsDir);

                // Process each attachment in the TNEF message
                foreach (MapiAttachment attachment in tnefMessage.Attachments)
                {
                    string fileName = string.IsNullOrEmpty(attachment.FileName) ? "UnnamedAttachment.bin" : attachment.FileName;
                    Console.WriteLine("Attachment Name: " + fileName);

                    string outputPath = Path.Combine(attachmentsDir, fileName);
                    try
                    {
                        attachment.Save(outputPath);
                        Console.WriteLine($"Saved attachment to: {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{fileName}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
