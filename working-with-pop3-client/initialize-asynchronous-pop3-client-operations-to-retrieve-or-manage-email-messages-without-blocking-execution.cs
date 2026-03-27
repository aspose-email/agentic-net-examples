using System;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Initialize the POP3 client with server details.
            using (Pop3Client client = new Pop3Client("pop.example.com", 110, "username", "password", SecurityOptions.Auto))
            {
                // Asynchronously retrieve the list of messages with main fields.
                Pop3MessageInfoCollection messages = await client.ListMessagesAsync(Pop3ListFields.Main);
                Console.WriteLine($"Total messages: {messages.Count}");

                // Iterate through each message info.
                foreach (Pop3MessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");

                    // Asynchronously fetch the full message and display the sender.
                    using (MailMessage message = await client.FetchMessageAsync(info.SequenceNumber))
                    {
                        Console.WriteLine($"From: {message.From}");
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
