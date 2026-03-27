using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // POP3 server connection settings
            string host = "pop.example.com";
            int port = 995;
            string username = "user@example.com";
            string password = "password";
            string subjectPattern = "Spam"; // messages containing this text in the subject will be deleted

            // Initialize POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Retrieve list of messages
                    Pop3MessageInfoCollection messages = client.ListMessages();

                    // Iterate through messages and mark those with matching subject for deletion
                    foreach (Pop3MessageInfo info in messages)
                    {
                        if (info.Subject != null && info.Subject.Contains(subjectPattern))
                        {
                            // DeleteMessage marks the message as deleted (by sequence number)
                            client.DeleteMessage(info.SequenceNumber);
                            Console.WriteLine($"Marked for deletion: UID={info.UniqueId}, Subject=\"{info.Subject}\"");
                        }
                    }

                    // Commit deletions so the server removes the marked messages
                    client.CommitDeletes();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("POP3 operation error: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
