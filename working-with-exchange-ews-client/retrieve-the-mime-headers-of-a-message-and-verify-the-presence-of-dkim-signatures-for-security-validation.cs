using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the EML file
            string emlPath = "sample.eml";

            // Verify that the file exists before attempting to load it
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"EML file not found: {emlPath}");
                return;
            }

            // Load the message inside a using block to ensure proper disposal
            using (MailMessage mailMessage = MailMessage.Load(emlPath))
            {
                // Display all MIME headers
                Console.WriteLine("MIME Headers:");
                foreach (string headerName in mailMessage.Headers.Keys)
                {
                    string headerValue = mailMessage.Headers[headerName];
                    Console.WriteLine($"{headerName}: {headerValue}");
                }

                // Check for the presence of a DKIM signature header
                bool hasDkimSignature = false;
                foreach (string headerName in mailMessage.Headers.Keys)
                {
                    if (headerName.Equals("DKIM-Signature", StringComparison.OrdinalIgnoreCase))
                    {
                        hasDkimSignature = true;
                        break;
                    }
                }

                // Output verification result
                if (hasDkimSignature)
                {
                    Console.WriteLine("DKIM signature header is present.");
                }
                else
                {
                    Console.WriteLine("DKIM signature header is NOT present.");
                }

                // Additional check using the IsSigned property (general signature detection)
                if (mailMessage.IsSigned)
                {
                    Console.WriteLine("The message is signed (any signature).");
                }
                else
                {
                    Console.WriteLine("The message is not signed.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
