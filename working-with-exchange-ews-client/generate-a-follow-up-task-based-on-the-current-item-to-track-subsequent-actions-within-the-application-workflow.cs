using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace FollowUpTaskSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder Exchange service URL and credentials
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Skip execution when placeholder values are detected
                if (serviceUrl.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder Exchange service URL detected. Skipping execution.");
                    return;
                }

                // Create the EWS client using the factory method
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    try
                    {
                        // Retrieve mailbox information
                        ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                        // List messages in the Inbox folder
                        ExchangeMessageInfoCollection messages = client.ListMessages(mailboxInfo.InboxUri);
                        if (messages == null || messages.Count == 0)
                        {
                            Console.WriteLine("No messages found in the Inbox.");
                            return;
                        }

                        // Fetch the first message to use its details for the follow‑up task
                        ExchangeMessageInfo firstInfo = messages[0];
                        using (MailMessage email = client.FetchMessage(firstInfo.UniqueUri))
                        {
                            // Create a follow‑up task based on the email
                            ExchangeTask followUpTask = new ExchangeTask
                            {
                                Subject = "Follow‑up: " + email.Subject,
                                Body = "Please follow up on the email received from " + (email.From.Count > 0 ? email.From[0].Address : "unknown sender") + ".",
                                StartDate = DateTime.Now,
                                DueDate = DateTime.Now.AddDays(2)
                            };

                            // Save the task to the default Tasks folder
                            client.CreateTask(followUpTask);
                            Console.WriteLine("Follow‑up task created successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error during Exchange operations: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unhandled exception: " + ex.Message);
            }
        }
    }
}
