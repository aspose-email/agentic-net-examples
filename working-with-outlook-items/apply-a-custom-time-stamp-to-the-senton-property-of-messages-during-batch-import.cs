using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

namespace AsposeEmailBatchImport
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – skip execution if they are not real.
                string exchangeHost = "exchange.example.com";
                string username = "user@example.com";
                string password = "password";

                if (exchangeHost.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder Exchange host detected. Skipping batch import.");
                    return;
                }

                // Folder that contains MSG files to import.
                string messagesFolder = "Messages";

                if (!Directory.Exists(messagesFolder))
                {
                    Console.Error.WriteLine($"Folder not found: {messagesFolder}");
                    return;
                }

                // Create the EWS client.
                IEWSClient client;
                try
                {
                    client = EWSClient.GetEWSClient(exchangeHost, username, password);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create or connect EWS client: {ex.Message}");
                    return;
                }

                using (client)
                {
                    // Process each MSG file in the folder.
                    string[] msgFiles = Directory.GetFiles(messagesFolder, "*.msg");
                    foreach (string msgFile in msgFiles)
                    {
                        if (!File.Exists(msgFile))
                        {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFile);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                            Console.Error.WriteLine($"File not found: {msgFile}");
                            continue;
                        }

                        try
                        {
                            using (MapiMessage mapiMessage = MapiMessage.Load(msgFile))
                            {
                                // Apply a custom timestamp to the message.
                                DateTime customTimestamp = new DateTime(2023, 12, 31, 23, 59, 59);
                                mapiMessage.ClientSubmitTime = customTimestamp;

                                // Append the message to the Sent Items folder (markAsSent = true).
                                try
                                {
                                    client.AppendMessage(mapiMessage, true);
                                    Console.WriteLine($"Imported and marked as sent: {Path.GetFileName(msgFile)}");
                                }
                                catch (Exception ex)
                                {
                                    Console.Error.WriteLine($"Failed to append message '{msgFile}': {ex.Message}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to load MSG file '{msgFile}': {ex.Message}");
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
