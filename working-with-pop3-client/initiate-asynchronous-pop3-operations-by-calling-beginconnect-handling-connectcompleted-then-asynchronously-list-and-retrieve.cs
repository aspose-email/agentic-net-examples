using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Tools.Search;

namespace Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                IAsyncPop3Client client = null;
                try
                {
                    client = await Pop3Client.CreateAsync(
                        host: "pop.example.com",
                        username: "user@example.com",
                        asyncTokenProvider: null,
                        port: 110,
                        securityOptions: SecurityOptions.Auto);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating POP3 client: {ex.Message}");
                    return;
                }

                using (client)
                {
                    Pop3MessageInfoCollection messageInfos = null;
                    try
                    {
                        messageInfos = await client.ListMessagesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error listing messages: {ex.Message}");
                        return;
                    }

                    foreach (Pop3MessageInfo info in messageInfos)
                    {
                        try
                        {
                            MailMessage mail = await client.FetchMessageAsync(info.UniqueId);
                            Console.WriteLine($"Subject: {mail.Subject}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error fetching message {info.UniqueId}: {ex.Message}");
                        }
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
