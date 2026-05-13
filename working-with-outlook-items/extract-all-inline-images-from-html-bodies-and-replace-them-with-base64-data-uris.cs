using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Input and output file paths
            string inputPath = "input.eml";
            string outputPath = "output.html";

            // Guard file existence
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

                Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
                return;
            }

            // Load the email message
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Ensure the message has an HTML body
                string htmlBody = message.HtmlBody;
                if (string.IsNullOrEmpty(htmlBody))
                {
                    Console.Error.WriteLine("Message does not contain an HTML body.");
                    return;
                }

                // Find all <img> tags with cid sources
                Regex imgCidRegex = new Regex(@"<img[^>]+src\s*=\s*[""']cid:(?<cid>[^""'>]+)[""']", RegexOptions.IgnoreCase);
                MatchCollection matches = imgCidRegex.Matches(htmlBody);
                if (matches.Count == 0)
                {
                    Console.WriteLine("No inline images with CID found.");
                }

                // Build a dictionary of ContentId -> base64 data URI for quick lookup
                Dictionary<string, string> cidToDataUri = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                foreach (Match match in matches)
                {
                    string cid = match.Groups["cid"].Value;
                    if (cidToDataUri.ContainsKey(cid))
                        continue; // Already processed

                    // Find the attachment with matching ContentId
                    Attachment matchingAttachment = null;
                    foreach (Attachment attachment in message.Attachments)
                    {
                        if (string.Equals(attachment.ContentId, cid, StringComparison.OrdinalIgnoreCase))
                        {
                            matchingAttachment = attachment;
                            break;
                        }
                    }

                    if (matchingAttachment == null)
                    {
                        Console.Error.WriteLine($"Attachment with Content-Id '{cid}' not found.");
                        continue;
                    }

                    // Read attachment bytes
                    byte[] attachmentBytes;
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        if (matchingAttachment.ContentStream != null)
                        {
                            matchingAttachment.ContentStream.CopyTo(memoryStream);
                        }
                        else
                        {
                            Console.Error.WriteLine($"Attachment '{cid}' does not have a content stream.");
                            continue;
                        }
                        attachmentBytes = memoryStream.ToArray();
                    }

                    // Determine MIME type (fallback to application/octet-stream)
                    string mimeType = matchingAttachment.ContentType?.MediaType ?? "application/octet-stream";

                    // Build data URI
                    string base64Data = Convert.ToBase64String(attachmentBytes);
                    string dataUri = $"data:{mimeType};base64,{base64Data}";
                    cidToDataUri[cid] = dataUri;
                }

                // Replace cid references with data URIs
                string updatedHtml = imgCidRegex.Replace(htmlBody, match =>
                {
                    string cid = match.Groups["cid"].Value;
                    if (cidToDataUri.TryGetValue(cid, out string dataUri))
                    {
                        // Preserve other attributes of the <img> tag
                        string prefix = match.Value.Substring(0, match.Value.IndexOf("src", StringComparison.OrdinalIgnoreCase));
                        string suffix = match.Value.Substring(match.Value.IndexOf('>', StringComparison.Ordinal));
                        return $"{prefix}src=\"{dataUri}\"{suffix}";
                    }
                    // If no data URI found, return the original match unchanged
                    return match.Value;
                });

                // Update the message's HTML body
                message.HtmlBody = updatedHtml;

                // Save the modified HTML to a file
                try
                {
                    File.WriteAllText(outputPath, updatedHtml, Encoding.UTF8);
                    Console.WriteLine($"Processed HTML saved to '{outputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write output file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
