using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials detection
            string host = "pop3.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = "output";
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            // Create and connect POP3 client
            using (Pop3Client client = new Pop3Client(host, username, password))
            {
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception connEx)
                {
                    Console.Error.WriteLine($"Failed to connect or authenticate: {connEx.Message}");
                    return;
                }

                // List messages on the server
                Pop3MessageInfoCollection messageInfos;
                try
                {
                    messageInfos = client.ListMessages();
                }
                catch (Exception listEx)
                {
                    Console.Error.WriteLine($"Failed to list messages: {listEx.Message}");
                    return;
                }

                foreach (Pop3MessageInfo info in messageInfos)
                {
                    // Fetch each message
                    using (MailMessage message = client.FetchMessage(info.SequenceNumber))
                    {
                        // Validate non‑empty Subject header
                        if (!string.IsNullOrWhiteSpace(message.Subject))
                        {
                            string safeSubject = SanitizeFileName(message.Subject);
                            string fileName = $"{info.SequenceNumber}_{safeSubject}.eml";
                            string filePath = Path.Combine(outputDirectory, fileName);

                            try
                            {
                                message.Save(filePath);
                                Console.WriteLine($"Saved message #{info.SequenceNumber} to \"{filePath}\"");
                            }
                            catch (Exception saveEx)
                            {
                                Console.Error.WriteLine($"Failed to save message #{info.SequenceNumber}: {saveEx.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Message #{info.SequenceNumber} skipped due to empty subject.");
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

    // Helper to remove invalid filename characters
    private static string SanitizeFileName(string name)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char c in name)
        {
            if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar || c == Path.VolumeSeparatorChar)
                continue;
            if (char.IsControl(c))
                continue;
            sb.Append(c);
        }
        string result = sb.ToString();
        return string.IsNullOrWhiteSpace(result) ? "NoSubject" : result;
    }
}
