using System;
using System.IO;
using System.Text;
using System.Net.Mime;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To = "receiver@example.com";
                message.Subject = "CSV Attachment Example";
                message.Body = "Please find the CSV data attached.";

                // CSV data to attach
                string csvContent = "Name,Age,Location\nJohn Doe,30,USA\nJane Smith,25,UK";

                // Create attachment with proper content type (text/csv)
                using (Attachment csvAttachment = Attachment.CreateAttachmentFromString(csvContent, "data.csv", Encoding.UTF8, "text/csv"))
                {
                    message.Attachments.Add(csvAttachment);
                }

                // Output file path
                string outputPath = "EmailWithCsv.eml";

                // Ensure the directory for the output file exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the email to disk
                message.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return;
        }
    }
}
