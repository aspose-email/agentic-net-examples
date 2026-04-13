using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the Outlook MSG file
            string filePath = "sample.msg";

            // Verify that the file exists before attempting to load it
            if (!File.Exists(filePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(filePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"File not found: {filePath}");
                return;
            }

            // Load the MSG file inside a using block to ensure proper disposal
            using (MapiMessage message = MapiMessage.Load(filePath))
            {
                // Retrieve the raw transport headers
                string transportHeaders = message.TransportMessageHeaders;

                if (string.IsNullOrEmpty(transportHeaders))
                {
                    Console.WriteLine("No transport headers found in the message.");
                    return;
                }

                // Split the headers into individual lines
                string[] headerLines = transportHeaders.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                // Log any "Received:" headers which represent routing information
                foreach (string headerLine in headerLines)
                {
                    if (headerLine.StartsWith("Received:", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine(headerLine.Trim());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Gracefully handle any unexpected errors
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
