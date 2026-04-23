using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace EmailBackup
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Connection parameters (replace with real values)
                string host = "imap.example.com";
                int port = 993;
                string username = "username";
                string password = "password";
                string outputFolder = "BackupSent";

                // Skip execution if placeholder credentials are detected
                if (host.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping backup operation.");
                    return;
                }

                // Ensure the output directory exists
                try
                {
                    if (!Directory.Exists(outputFolder))
                    {
                        Directory.CreateDirectory(outputFolder);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to prepare output folder: {ex.Message}");
                    return;
                }

                // Connect to the IMAP server and back up messages from the Sent folder
                using (ImapClient client = new ImapClient(host, port, SecurityOptions.Auto))
                {
                    try
                    {
                        client.Username = username;
                        client.Password = password;

                        // Select the Sent folder
                        client.SelectFolder("Sent");

                        // Retrieve the list of messages in the folder
                        ImapMessageInfoCollection messages = client.ListMessages();

                        foreach (ImapMessageInfo messageInfo in messages)
                        {
                            try
                            {
                                // Fetch the full message
                                MailMessage mail = client.FetchMessage(messageInfo.UniqueId);

                                // Build the file path for the MSG file
                                string filePath = Path.Combine(outputFolder, $"{messageInfo.UniqueId}.msg");

                                // Save the message as MSG (Unicode format)
                                mail.Save(filePath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode));

                                // Preserve the original message date as file timestamps
                                if (mail.Date != DateTime.MinValue)
                                {
                                    File.SetCreationTime(filePath, mail.Date);
                                    File.SetLastWriteTime(filePath, mail.Date);
                                }
                            }
                            catch (Exception exMessage)
                            {
                                Console.Error.WriteLine($"Failed to back up message {messageInfo.UniqueId}: {exMessage.Message}");
                            }
                        }
                    }
                    catch (Exception exClient)
                    {
                        Console.Error.WriteLine($"IMAP operation failed: {exClient.Message}");
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
