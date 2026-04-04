using System;
using System.IO;
using Aspose.Email;
class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.msg";
            string outputDirectory = "output";

            // Guard input file existence
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
                        placeholder.Save(inputPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                Directory.CreateDirectory(outputDirectory);
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Load the message
            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(inputPath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load message: {loadEx.Message}");
                return;
            }

            using (mailMessage)
            {
                int resourceIndex = 0;

                foreach (AlternateView alternateView in mailMessage.AlternateViews)
                {
                    foreach (LinkedResource linkedResource in alternateView.LinkedResources)
                    {
                        resourceIndex++;

                        // Extract the binary data of the linked resource
                        Stream contentStream = linkedResource.ContentStream;
                        if (contentStream == null)
                        {
                            Console.Error.WriteLine($"Linked resource #{resourceIndex} has no content stream.");
                            continue;
                        }

                        byte[] resourceData;
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            try
                            {
                                contentStream.Position = 0;
                                contentStream.CopyTo(memoryStream);
                                resourceData = memoryStream.ToArray();
                            }
                            catch (Exception copyEx)
                            {
                                Console.Error.WriteLine($"Failed to read linked resource #{resourceIndex}: {copyEx.Message}");
                                continue;
                            }
                        }

                        // Determine a file name for the resource
                        string resourceFileName = GetResourceFileName(linkedResource, resourceIndex);

                        // Create a new message that contains the extracted image as an attachment
                        using (MailMessage imageMessage = new MailMessage())
                        {
                            imageMessage.From = "placeholder@example.com";
                            imageMessage.To = "placeholder@example.com";
                            imageMessage.Subject = $"Embedded Image {resourceIndex}";
                            using (Attachment attachment = new Attachment(new MemoryStream(resourceData), resourceFileName))
                            {
                                imageMessage.Attachments.Add(attachment);

                                string outputPath = Path.Combine(outputDirectory, $"EmbeddedImage_{resourceIndex}.msg");
                                try
                                {
                                    imageMessage.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                                    Console.WriteLine($"Saved embedded image #{resourceIndex} to {outputPath}");
                                }
                                catch (Exception saveEx)
                                {
                                    Console.Error.WriteLine($"Failed to save MSG for resource #{resourceIndex}: {saveEx.Message}");
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static string GetResourceFileName(LinkedResource resource, int index)
    {
        string baseName = resource.ContentId ?? $"resource_{index}";
        string extension = ".bin";

        if (resource.ContentType != null && !string.IsNullOrEmpty(resource.ContentType.MediaType))
        {
            string media = resource.ContentType.MediaType.ToLowerInvariant();
            if (media.Contains("jpeg") || media.Contains("jpg"))
                extension = ".jpg";
            else if (media.Contains("png"))
                extension = ".png";
            else if (media.Contains("gif"))
                extension = ".gif";
            else if (media.Contains("bmp"))
                extension = ".bmp";
        }

        // Remove any characters that are invalid in file names
        foreach (char invalidChar in Path.GetInvalidFileNameChars())
        {
            baseName = baseName.Replace(invalidChar.ToString(), "_");
        }

        return baseName + extension;
    }
}
