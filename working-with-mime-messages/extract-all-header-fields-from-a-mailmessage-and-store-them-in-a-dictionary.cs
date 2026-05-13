using Aspose.Email.Mime;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "sample.eml";

            // Ensure the file exists; create a minimal placeholder if it does not.
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

                try
                {
                    using (StreamWriter writer = new StreamWriter(emlPath, false))
                    {
                        writer.WriteLine("From: sender@example.com");
                        writer.WriteLine("To: recipient@example.com");
                        writer.WriteLine("Subject: Sample Email");
                        writer.WriteLine("Date: Thu, 1 Jan 1970 00:00:00 +0000");
                        writer.WriteLine();
                        writer.WriteLine("This is a placeholder email body.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the email message.
            using (MailMessage message = MailMessage.Load(emlPath))
            {
                HeaderCollection headers = message.Headers;
                Dictionary<string, string> headerDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                // Iterate over header keys and store them in the dictionary.
                foreach (string key in headers.Keys)
                {
                    string value = headers[key];
                    headerDictionary[key] = value;
                }

                // Output the extracted headers.
                foreach (KeyValuePair<string, string> kvp in headerDictionary)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
