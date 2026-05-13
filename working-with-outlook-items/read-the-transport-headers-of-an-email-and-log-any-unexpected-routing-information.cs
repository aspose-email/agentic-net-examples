using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string messagePath = "sample.msg";

            // Ensure the file exists; create a minimal placeholder if it does not.
            if (!File.Exists(messagePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(messagePath);
                        Console.WriteLine($"Created placeholder message at '{messagePath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the message and read transport headers.
            try
            {
                using (MapiMessage message = MapiMessage.Load(messagePath))
                {
                    string transportHeaders = message.TransportMessageHeaders;

                    if (string.IsNullOrEmpty(transportHeaders))
                    {
                        Console.WriteLine("No transport headers found in the message.");
                        return;
                    }

                    // Split headers into individual lines.
                    string[] headerLines = transportHeaders.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    List<string> unexpectedHeaders = new List<string>();

                    foreach (string line in headerLines)
                    {
                        string trimmed = line.Trim();

                        // Consider "Received:" lines as expected routing information.
                        if (!trimmed.StartsWith("Received:", StringComparison.OrdinalIgnoreCase))
                        {
                            unexpectedHeaders.Add(trimmed);
                        }
                    }

                    if (unexpectedHeaders.Count == 0)
                    {
                        Console.WriteLine("All transport headers are expected (only Received lines).");
                    }
                    else
                    {
                        Console.WriteLine("Unexpected transport headers detected:");
                        foreach (string unexpected in unexpectedHeaders)
                        {
                            Console.WriteLine($"  {unexpected}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing message file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
