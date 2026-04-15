using System;
using System.IO;
using System.Text;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Input and output paths
            string inputPath = "input.eml";
            string outputPath = "output.eml";

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
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Load the email message
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Process only if there is an HTML body
                if (string.IsNullOrEmpty(message.HtmlBody))
                {
                    Console.WriteLine("Message does not contain an HTML body. No processing needed.");
                }
                else
                {
                    string html = message.HtmlBody;

                    // Replace each inline linked resource with a base64 data URI
                    foreach (LinkedResource resource in message.LinkedResources)
                    {
                        // Read the resource data
                        using (MemoryStream ms = new MemoryStream())
                        {
                            resource.ContentStream.CopyTo(ms);
                            byte[] bytes = ms.ToArray();
                            string base64 = Convert.ToBase64String(bytes);

                            // Determine media type (fallback to generic octet-stream)
                            string mediaType = resource.ContentType?.MediaType ?? "application/octet-stream";

                            // Build data URI
                            string dataUri = $"data:{mediaType};base64,{base64}";

                            // Replace cid reference in HTML
                            string cidReference = $"cid:{resource.ContentId}";
                            html = html.Replace(cidReference, dataUri);
                        }
                    }

                    // Update the message's HTML body
                    message.HtmlBody = html;
                }

                // Save the modified message
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved to {outputPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save message: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
