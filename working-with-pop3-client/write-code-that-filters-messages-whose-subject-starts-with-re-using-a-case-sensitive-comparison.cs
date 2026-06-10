using Aspose.Email;
using System;
using System.Collections.Generic;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Tools.Search;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – skip actual network call in CI environments
                string host = "pop3.example.com";
                string username = "user@example.com";
                string password = "password";

                if (host.Contains("example.com") || username.Contains("example.com") || string.IsNullOrWhiteSpace(password))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping POP3 connection.");
                    return;
                }

                // Create and use the POP3 client
                using (Pop3Client client = new Pop3Client(host, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Validate connection credentials
                        client.ValidateCredentials();

                        // Retrieve all messages
                        Pop3MessageInfoCollection allMessages = client.ListMessages();

                        // Filter messages whose subject starts with "Re:" (case‑sensitive)
                        List<Pop3MessageInfo> filtered = new List<Pop3MessageInfo>();
                        foreach (Pop3MessageInfo info in allMessages)
                        {
                            if (info.Subject != null && info.Subject.StartsWith("Re:", StringComparison.Ordinal))
                            {
                                filtered.Add(info);
                            }
                        }

                        // Output filtered subjects
                        Console.WriteLine($"Found {filtered.Count} message(s) with subject starting with \"Re:\"");
                        foreach (Pop3MessageInfo info in filtered)
                        {
                            Console.WriteLine($"- {info.Subject}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
