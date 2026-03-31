using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the document (e.g., an MSG file) containing custom metadata.
            string documentPath = "sample.msg";

            // Guard file existence.
            if (!File.Exists(documentPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(documentPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"The file '{documentPath}' does not exist.");
                return;
            }

            // Load the message from the file.
            using (MailMessage message = MailMessage.Load(documentPath))
            {
                // Iterate through all headers and output those that represent custom metadata.
                // Conventionally, custom metadata headers start with "X-".
                foreach (string headerName in message.Headers.AllKeys)
                {
                    if (headerName.StartsWith("X-", StringComparison.OrdinalIgnoreCase))
                    {
                        string headerValue = message.Headers[headerName];
                        Console.WriteLine($"{headerName}: {headerValue}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
