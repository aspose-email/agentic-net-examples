using Aspose.Email.Tools.Search;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder POP3 server credentials
            string host = "pop3.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip actual network call when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping POP3 connection.");
                return;
            }

            // Create and connect POP3 client
            using (Pop3Client client = new Pop3Client(host, username, password))
            {
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to validate credentials: {ex.Message}");
                    return;
                }

                // Build a query to filter messages (e.g., subject contains "Invoice")
                MailQueryBuilder builder = new MailQueryBuilder();
                builder.Subject.Contains("Invoice");
                MailQuery query = builder.GetQuery();

                // Retrieve filtered message infos
                Pop3MessageInfoCollection infos;
                try
                {
                    infos = client.ListMessages(query);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error listing messages: {ex.Message}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = "output";
                try
                {
                    if (!Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }

                // Process each message
                foreach (Pop3MessageInfo info in infos)
                {
                    try
                    {
                        using (MailMessage message = client.FetchMessage(info.SequenceNumber))
                        {
                            Console.WriteLine($"Processing message #{info.SequenceNumber}: {message.Subject}");

                            // Save the message to a file
                            string filePath = Path.Combine(outputDir, $"msg_{info.SequenceNumber}.eml");
                            try
                            {
                                message.Save(filePath);
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save message #{info.SequenceNumber}: {ex.Message}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to fetch message #{info.SequenceNumber}: {ex.Message}");
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
