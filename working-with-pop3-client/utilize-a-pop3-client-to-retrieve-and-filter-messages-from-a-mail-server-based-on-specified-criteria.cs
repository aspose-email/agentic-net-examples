using System;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Mime;

namespace Pop3RetrieveAndFilter
{
    class Program
    {
        static void Main()
        {
            // POP3 server connection parameters (replace with real values)
            const string host = "pop3.example.com";
            const int port = 110; // use 995 for SSL
            const string username = "user@example.com";
            const string password = "password";

            // Guard: skip network calls when placeholder credentials are detected
            bool placeholders = host.Contains("example.com") ||
                                username.Contains("example.com") ||
                                password.Equals("password", StringComparison.OrdinalIgnoreCase);

            if (placeholders)
            {
                Console.WriteLine("Placeholder credentials detected. Skipping POP3 operations.");
                return;
            }

            try
            {
                using (Pop3Client client = new Pop3Client(host, port, username, password))
                {
                    // Retrieve the list of all messages
                    Pop3MessageInfoCollection allInfos = client.ListMessages();

                    Console.WriteLine($"Total messages on server: {allInfos.Count}");

                    const string subjectFilter = "Invoice";
                    int matchedCount = 0;

                    // Iterate through messages and apply the filter
                    for (int i = 0; i < allInfos.Count; i++)
                    {
                        Pop3MessageInfo info = allInfos[i];
                        MailMessage message = client.FetchMessage(info.UniqueId);

                        if (!string.IsNullOrEmpty(message.Subject) &&
                            message.Subject.IndexOf(subjectFilter, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            matchedCount++;
                            Console.WriteLine($"--- Message {matchedCount} ---");
                            Console.WriteLine($"Subject : {message.Subject}");
                            Console.WriteLine($"From    : {message.From}");
                            Console.WriteLine($"Date    : {message.Date}");
                            Console.WriteLine();
                        }
                    }

                    Console.WriteLine($"Found {matchedCount} message(s) matching the criteria.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
