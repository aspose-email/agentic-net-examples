using System;
using System.Net;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // IMAP server connection parameters (replace with real values)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Create and connect the IMAP client
            using (Aspose.Email.Clients.Imap.ImapClient client = new Aspose.Email.Clients.Imap.ImapClient(
                host,
                port,
                username,
                password,
                Aspose.Email.Clients.SecurityOptions.Auto))
            {
                // Prepare thread search conditions (default fetches all threads)
                Aspose.Email.Clients.Imap.XGMThreadSearchConditions conditions = new Aspose.Email.Clients.Imap.XGMThreadSearchConditions();

                // Retrieve conversation threads synchronously
                List<Aspose.Email.Clients.Imap.MessageThreadResult> threads =
                    client.GetMessageThreadsAsync(conditions).GetAwaiter().GetResult();

                // Output each thread hierarchy
                foreach (Aspose.Email.Clients.Imap.MessageThreadResult thread in threads)
                {
                    PrintThread(thread, 0);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }

    // Recursively prints a thread and its child messages with indentation
    static void PrintThread(Aspose.Email.Clients.Imap.MessageThreadResult threadResult, int indentLevel)
    {
        string indent = new string(' ', indentLevel * 2);
        Console.WriteLine($"{indent}ConversationId: {threadResult.ConversationId}");
        Console.WriteLine($"{indent}UniqueId: {threadResult.UniqueId}");

        foreach (Aspose.Email.Clients.Imap.MessageThreadResult child in threadResult.ChildMessages)
        {
            PrintThread(child, indentLevel + 1);
        }
    }
}