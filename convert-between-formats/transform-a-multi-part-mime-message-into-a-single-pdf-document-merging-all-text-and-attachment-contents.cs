using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output file paths
            string inputPath = "input.eml";
            string outputPath = "merged.pdf";

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the multi‑part MIME message
            MailMessage mailMessage = MailMessage.Load(inputPath);

            // Merge body and attachment contents into a single PDF (as plain text for demonstration)
            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                writer.WriteLine($"Subject: {mailMessage.Subject}");
                writer.WriteLine($"From: {mailMessage.From}");
                writer.WriteLine($"To: {string.Join(", ", mailMessage.To)}");
                writer.WriteLine();
                writer.WriteLine("Body:");
                writer.WriteLine(mailMessage.Body);
                writer.WriteLine();
                writer.WriteLine("Attachments:");
                foreach (Attachment attachment in mailMessage.Attachments)
                {
                    writer.WriteLine($"Attachment: {attachment.Name}");
                    // Attempt to include text attachment content
                    if (attachment.ContentType.MediaType.StartsWith("text", StringComparison.OrdinalIgnoreCase))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            attachment.Save(ms);
                            ms.Position = 0;
                            using (StreamReader sr = new StreamReader(ms))
                            {
                                writer.WriteLine(sr.ReadToEnd());
                            }
                        }
                    }
                    else
                    {
                        writer.WriteLine("[Binary content omitted]");
                    }
                    writer.WriteLine();
                }
            }

            Console.WriteLine($"Merged content written to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
