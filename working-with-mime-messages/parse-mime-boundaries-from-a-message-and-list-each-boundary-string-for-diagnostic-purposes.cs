using System;
using System.IO;
using System.Text.RegularExpressions;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the EML file to be analyzed
            string emlPath = "sample.eml";

            // Guard against missing file
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

                Console.Error.WriteLine($"File not found: {emlPath}");
                return;
            }

            // Load the message inside a using block to ensure disposal
            using (MailMessage message = MailMessage.Load(emlPath))
            {
                // Retrieve the Content-Type header which may contain boundary definitions
                string contentTypeHeader = message.Headers[HeaderType.ContentType];

                if (string.IsNullOrEmpty(contentTypeHeader))
                {
                    Console.WriteLine("No Content-Type header found in the message.");
                    return;
                }

                // Find all boundary parameters using a regular expression
                MatchCollection matches = Regex.Matches(
                    contentTypeHeader,
                    @"boundary\s*=\s*(""([^""]+)""|([^;]+))",
                    RegexOptions.IgnoreCase);

                if (matches.Count == 0)
                {
                    Console.WriteLine("No MIME boundaries detected in the Content-Type header.");
                    return;
                }

                int index = 1;
                foreach (Match match in matches)
                {
                    // Extract the boundary value, handling quoted and unquoted forms
                    string boundary = match.Groups[2].Success
                        ? match.Groups[2].Value
                        : match.Groups[3].Value.Trim();

                    Console.WriteLine($"Boundary {index}: {boundary}");
                    index++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
