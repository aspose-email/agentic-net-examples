using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Pop3DeleteBySubject
{
    class Program
    {
        static void Main()
        {
            // Author note: Example demonstrates deleting POP3 messages whose subject matches a pattern.
            string host = "pop.example.com";
            int port = 110; // Change to 995 for SSL
            string username = "user@example.com";
            string password = "password";

            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            SecurityOptions security = SecurityOptions.Auto; // Adjust as needed
            string subjectPattern = "Spam"; // Messages containing this text in the subject will be deleted

            try
            {
                using (Pop3Client pop3Client = new Pop3Client(host, port, username, password, security))
                {
                    int messageCount = pop3Client.GetMessageCount();

                    for (int i = 1; i <= messageCount; i++)
                    {
                        Pop3MessageInfo info = pop3Client.GetMessageInfo(i);
                        if (info != null && !string.IsNullOrEmpty(info.Subject) && info.Subject.Contains(subjectPattern))
                        {
                            pop3Client.DeleteMessage(i);
                            Console.WriteLine($"Deleted message #{i} with subject: \"{info.Subject}\"");
                        }
                    }

                    // Commit deletions so the server removes the marked messages.
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
                return;
            }
        }
    }
}
