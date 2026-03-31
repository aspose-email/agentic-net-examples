using System;
using System.Collections.Generic;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details – replace with real values.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected.
            if (mailboxUri.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create the EWS client inside a using block to ensure disposal.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Prepare deletion options – move items to Deleted Items folder.
                    DeletionOptions deleteOptions = new DeletionOptions(DeletionType.MoveToDeletedItems);

                    // ---------- Single item deletion ----------
                    // URI of the message to delete.
                    string singleMessageUri = "https://exchange.example.com/EWS/Exchange.asmx/Message/AAAkAD...";

                    // Delete a single message.
                    client.DeleteItem(singleMessageUri, deleteOptions);
                    Console.WriteLine("Single message deleted successfully.");

                    // ---------- Batch deletion ----------
                    // URIs of the messages to delete.
                    List<string> messageUris = new List<string>
                    {
                        "https://exchange.example.com/EWS/Exchange.asmx/Message/AAAkAD...1",
                        "https://exchange.example.com/EWS/Exchange.asmx/Message/AAAkAD...2",
                        "https://exchange.example.com/EWS/Exchange.asmx/Message/AAAkAD...3"
                    };

                    // Delete multiple messages in one call.
                    client.DeleteItems(messageUris, deleteOptions);
                    Console.WriteLine("Batch messages deleted successfully.");
                }
                catch (Exception ex)
                {
                    // Handle errors that occur during client operations.
                    Console.Error.WriteLine($"Error during deletion: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception guard.
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
