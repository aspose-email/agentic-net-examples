using Aspose.Email;
using Aspose.Email.Mapi;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace EmailColorExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the Outlook MSG file
                string msgPath = @"c:\outlookmessage.msg";

                // Ensure the directory for the MSG file exists
                string? directory = Path.GetDirectoryName(msgPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Verify that the file exists before attempting to load
                if (!File.Exists(msgPath))
                {
                    try
                    {
                        using (MapiMessage placeholder = new MapiMessage(
                            "from@example.com",
                            "to@example.com",
                            "Placeholder Subject",
                            "Placeholder body."))
                        {
                            placeholder.Save(msgPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                        return;
                    }

                    Console.Error.WriteLine($"Input file not found. Placeholder MSG created at: {msgPath}");
                    return;
                }

                // Load the MSG file
                MapiMessage msg = MapiMessage.Load(msgPath);

                // Get the message body (may contain HTML)
                string body = msg.Body ?? string.Empty;

                // Regular expression to find hex color codes in the body (e.g., #FF0000 or #F00)
                Regex colorRegex = new Regex(@"#([0-9a-fA-F]{3}|[0-9a-fA-F]{6})", RegexOptions.Compiled);

                // Find all matches
                MatchCollection matches = colorRegex.Matches(body);

                if (matches.Count == 0)
                {
                    Console.WriteLine("No color attributes found in the email body.");
                }
                else
                {
                    Console.WriteLine("Extracted color values:");
                    foreach (Match match in matches)
                    {
                        // Output the full match (including the leading '#')
                        Console.WriteLine(match.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected errors
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
