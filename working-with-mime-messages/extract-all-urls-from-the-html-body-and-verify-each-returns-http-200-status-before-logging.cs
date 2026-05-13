using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the email file (EML)
            string emailPath = "email.eml";

            // Verify the email file exists before attempting to load
            if (!File.Exists(emailPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emailPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Email file not found: {emailPath}");
                return;
            }

            // Load the email message
            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(emailPath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load email: {loadEx.Message}");
                return;
            }

            using (mailMessage)
            {
                // Ensure the message has an HTML body
                string htmlBody = mailMessage.HtmlBody;
                if (string.IsNullOrEmpty(htmlBody))
                {
                    Console.WriteLine("No HTML body found in the email.");
                    return;
                }

                // Extract URLs using a regular expression
                Regex urlRegex = new Regex(@"https?://[^\s\""]+", RegexOptions.IgnoreCase);
                MatchCollection matches = urlRegex.Matches(htmlBody);
                List<string> urlList = new List<string>();
                foreach (Match match in matches)
                {
                    if (match.Success)
                    {
                        urlList.Add(match.Value);
                    }
                }

                if (urlList.Count == 0)
                {
                    Console.WriteLine("No URLs found in the HTML body.");
                    return;
                }

                // Verify each URL returns HTTP 200
                using (HttpClient httpClient = new HttpClient())
                {
                    foreach (string url in urlList)
                    {
                        try
                        {
                            HttpResponseMessage response = httpClient.GetAsync(url).Result;
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                Console.WriteLine($"URL OK: {url}");
                            }
                            else
                            {
                                Console.WriteLine($"URL returned {((int)response.StatusCode)}: {url}");
                            }
                        }
                        catch (Exception httpEx)
                        {
                            Console.WriteLine($"Failed to reach URL {url}: {httpEx.Message}");
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
