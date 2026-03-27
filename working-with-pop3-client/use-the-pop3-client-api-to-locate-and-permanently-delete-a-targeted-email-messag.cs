using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Pop3DeleteExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // POP3 server connection settings (replace with real values)
                string host = "pop3.example.com";
                int port = 995;
                string username = "user@example.com";
                string password = "password";

                // Initialize the POP3 client
                using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                {
                    // Retrieve the list of messages in the mailbox
                    Pop3MessageInfoCollection messages = client.ListMessages();

                    // Define the criteria to locate the target message (e.g., by subject)
                    string targetSubjectKeyword = "Target Subject";

                    foreach (Pop3MessageInfo info in messages)
                    {
                        if (!string.IsNullOrEmpty(info.Subject) && info.Subject.Contains(targetSubjectKeyword))
                        {
                            // Mark the message for deletion using its sequence number
                            client.DeleteMessage(info.SequenceNumber);

                            // Permanently remove all messages marked as deleted
                            client.CommitDeletes();

                            Console.WriteLine($"Deleted message with subject: {info.Subject}");
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
