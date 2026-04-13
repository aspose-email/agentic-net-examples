using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Net;
using Aspose.Email;
class Program
{
    static void Main()
    {
        try
        {
            // Prepare connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create Exchange client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // List messages in the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                if (messages == null || messages.Count == 0)
                {
                    Console.Error.WriteLine("No messages found in the Inbox.");
                    return;
                }

                // Take the first message
                ExchangeMessageInfo messageInfo = messages[0];
                string messageUri = messageInfo.UniqueUri;

                // Save raw MIME to a memory stream
                using (MemoryStream mimeStream = new MemoryStream())
                {
                    try
                    {
                        client.SaveMessage(messageUri, mimeStream);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                        return;
                    }

                    // Reset stream position for reading
                    mimeStream.Position = 0;

                    // Optionally write raw MIME to a file for inspection
                    string rawFilePath = "rawMessage.eml";

                    // Skip external calls when placeholder credentials are used
                    if (mailboxUri.Contains("example.com"))
                    {
                        Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                        return;
                    }

                    try
                    {
                        string directory = Path.GetDirectoryName(rawFilePath);
                        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        using (FileStream fileStream = new FileStream(rawFilePath, FileMode.Create, FileAccess.Write))
                        {
                            mimeStream.CopyTo(fileStream);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"File I/O error: {ex.Message}");
                        return;
                    }

                    // Read MIME content as text
                    mimeStream.Position = 0;
                    string mimeContent;
                    using (StreamReader reader = new StreamReader(mimeStream, Encoding.UTF8))
                    {
                        mimeContent = reader.ReadToEnd();
                    }

                    // Find all base64 encoded attachment parts
                    List<string> base64Blocks = new List<string>();
                    string pattern = @"Content-Transfer-Encoding:\s*base64\s*(?:\r?\n)+([\s\S]*?)(?:\r?\n\r?\n|$)";
                    foreach (Match match in Regex.Matches(mimeContent, pattern, RegexOptions.IgnoreCase))
                    {
                        string block = match.Groups[1].Value;
                        // Remove line breaks
                        string cleaned = Regex.Replace(block, @"\s+", "");
                        if (!string.IsNullOrEmpty(cleaned))
                        {
                            base64Blocks.Add(cleaned);
                        }
                    }

                    // Decode each base64 block and display its size
                    for (int i = 0; i < base64Blocks.Count; i++)
                    {
                        try
                        {
                            byte[] decodedBytes = Convert.FromBase64String(base64Blocks[i]);
                            Console.WriteLine($"Attachment {i + 1}: decoded size = {decodedBytes.Length} bytes");
                            // Additional analysis can be performed on decodedBytes here
                        }
                        catch (FormatException)
                        {
                            Console.Error.WriteLine($"Attachment {i + 1}: invalid base64 data.");
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
}
