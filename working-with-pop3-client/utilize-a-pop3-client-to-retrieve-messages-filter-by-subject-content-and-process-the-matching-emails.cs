using System;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Mime;

namespace Pop3Sample
{
    class Program
    {
        static void Main()
        {
            // Placeholder credentials – replace with real values or skip execution.
            const string host = "your_host";
            const string username = "your_username";
            const string password = "your_password";

            // Guard against executing network calls with placeholder data.
            if (host.StartsWith("your_") || username.StartsWith("your_") || password.StartsWith("your_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. POP3 operations are skipped.");
                return;
            }

            // Subject filter – only messages containing this text will be processed.
            const string subjectFilter = "Invoice";

            try
            {
                using (Pop3Client pop3Client = new Pop3Client(host, username, password))
                {
                    // Get total number of messages in the mailbox.
                    int messageCount = pop3Client.GetMessageCount();

                    for (int i = 1; i <= messageCount; i++)
                    {
                        // Retrieve basic info for each message.
                        Pop3MessageInfo messageInfo = pop3Client.GetMessageInfo(i);

                        // Ensure the Subject property is available before checking.
                        if (!string.IsNullOrEmpty(messageInfo.Subject) &&
                            messageInfo.Subject.IndexOf(subjectFilter, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            // Fetch the full message.
                            MailMessage mailMessage = pop3Client.FetchMessage(i);

                            // Process the matching email – here we simply output key details.
                            Console.WriteLine("----- Matching Message -----");
                            Console.WriteLine($"Subject : {mailMessage.Subject}");
                            Console.WriteLine($"From    : {mailMessage.From}");
                            Console.WriteLine($"Date    : {mailMessage.Date}");
                            Console.WriteLine($"Body    : {mailMessage.Body}");
                            Console.WriteLine("----------------------------");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred while accessing POP3 server: {ex.Message}");
            }
        }
    }
}
