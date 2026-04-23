using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string exchangeUri = "https://example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";
            string messageUri = "/mailfolders/inbox/messages/123";

            // Output file paths
            string emlPath = "fetchedMessage.eml";
            string htmlPath = "fetchedMessage.html";

            // Guard against placeholder credentials to avoid real network calls
            if (username == "username" && password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operation.");
                return;
            }

            // Ensure the output directory exists
            try
            {
                string emlDirectory = Path.GetDirectoryName(emlPath);
                if (!string.IsNullOrEmpty(emlDirectory) && !Directory.Exists(emlDirectory))
                {
                    Directory.CreateDirectory(emlDirectory);
                }

                string htmlDirectory = Path.GetDirectoryName(htmlPath);
                if (!string.IsNullOrEmpty(htmlDirectory) && !Directory.Exists(htmlDirectory))
                {
                    Directory.CreateDirectory(htmlDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directories: {dirEx.Message}");
                return;
            }

            // Fetch the message and save it as EML
            try
            {
                using (ExchangeClient client = new ExchangeClient(exchangeUri, username, password))
                {
                    client.SaveMessage(messageUri, emlPath);
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"Failed to fetch or save the message: {clientEx.Message}");
                return;
            }

            // Verify the EML file exists before loading
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

                Console.Error.WriteLine($"EML file not found at path: {emlPath}");
                return;
            }

            // Load the fetched message
            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(emlPath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load EML file: {loadEx.Message}");
                return;
            }

            // Save the HTML body preserving original markup and styling
            try
            {
                using (mailMessage)
                {
                    HtmlSaveOptions htmlOptions = new HtmlSaveOptions();
                    mailMessage.Save(htmlPath, htmlOptions);
                }
            }
            catch (Exception saveEx)
            {
                Console.Error.WriteLine($"Failed to save HTML file: {saveEx.Message}");
                return;
            }

            Console.WriteLine($"HTML body saved successfully to: {htmlPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
