using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "sample.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Load the MSG file as a MailMessage
            using (MailMessage mailMessage = MailMessage.Load(msgPath))
            {
                string outputDir = "LinkedResources";

                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                int index = 0;
                foreach (LinkedResource linkedResource in mailMessage.LinkedResources)
                {
                    string extension = GetExtension(linkedResource.ContentType.MediaType);
                    string filePath = Path.Combine(outputDir, $"resource_{index}{extension}");

                    try
                    {
                        linkedResource.Save(filePath);
                        Console.WriteLine($"Saved linked resource to {filePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving linked resource: {ex.Message}");
                    }

                    index++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    static string GetExtension(string mediaType)
    {
        if (string.IsNullOrEmpty(mediaType))
            return string.Empty;

        switch (mediaType.ToLowerInvariant())
        {
            case "image/jpeg":
                return ".jpg";
            case "image/png":
                return ".png";
            case "image/gif":
                return ".gif";
            case "text/plain":
                return ".txt";
            case "text/html":
                return ".html";
            default:
                return ".bin";
        }
    }
}
