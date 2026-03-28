using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Initialize and connect the IMAP client
            try
            {
                using (ImapClient client = new ImapClient("imap.example.com", 993, "username", "password", SecurityOptions.Auto))
                {
                    // Retrieve the list of messages in the selected folder
                    try
                    {
                        ImapMessageInfoCollection messages = client.ListMessages();
                        if (messages != null && messages.Count > 0)
                        {
                            // Get the first message (or any specific one by index)
                            ImapMessageInfo messageInfo = messages[0];

                            // Obtain the flags of the message
                            ImapMessageFlags flags = messageInfo.Flags;

                            Console.WriteLine($"Subject: {messageInfo.Subject}");
                            Console.WriteLine($"Flags: {flags}");
                        }
                        else
                        {
                            Console.WriteLine("No messages found in the mailbox.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error retrieving messages: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error connecting to IMAP server: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
