using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder connection details – replace with real values.
            string serverUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against executing with placeholder credentials.
            if (serverUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Initialize the Exchange client.
            using (ExchangeClient client = new ExchangeClient(serverUri, username, password))
            {
                // Placeholder message URI – replace with the actual URI of the email.
                string messageUri = "/mail/inbox/12345";

                // Fetch the email message.
                using (MailMessage message = client.FetchMessage(messageUri))
                {
                    // Retrieve the HTML body of the message.
                    string htmlBody = message.HtmlBody;

                    // Define the output file path.
                    string outputPath = "email.html";

                    // Ensure the output directory exists.
                    string outputDir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    // Write the HTML markup to the file.
                    try
                    {
                        File.WriteAllText(outputPath, htmlBody);
                        Console.WriteLine($"HTML body saved to {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to write HTML file: {ex.Message}");
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
