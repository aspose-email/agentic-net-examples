using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Storage.Zimbra;

namespace AsposeEmailZimbraSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // ---------- TGZ (Zimbra) handling ----------
                string tgzFilePath = "archive.tgz";

                if (!File.Exists(tgzFilePath))
                {
                    Console.Error.WriteLine($"TGZ file not found at path: {tgzFilePath}");
                }
                else
                {
                    try
                    {
                        using (TgzReader tgzReader = new TgzReader(tgzFilePath))
                        {
                            // Read messages until no more are available
                            while (true)
                            {
                                try
                                {
                                    tgzReader.ReadNextMessage();
                                }
                                catch (EndOfStreamException)
                                {
                                    // Reached the end of the TGZ archive
                                    break;
                                }

                                MailMessage currentMessage = tgzReader.CurrentMessage;
                                if (currentMessage != null)
                                {
                                    Console.WriteLine($"[TGZ] Subject: {currentMessage.Subject}");
                                    Console.WriteLine($"[TGZ] From: {currentMessage.From}");
                                    Console.WriteLine($"[TGZ] Date: {currentMessage.Date}");
                                    Console.WriteLine();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing TGZ file: {ex.Message}");
                    }
                }

                // ---------- IMAP email handling ----------
                string imapHost = "imap.example.com";
                int imapPort = 993;
                string imapUsername = "user@example.com";
                string imapPassword = "password";

                try
                {
                    using (ImapClient imapClient = new ImapClient(imapHost, imapPort, imapUsername, imapPassword))
                    {
                        imapClient.SecurityOptions = SecurityOptions.SSLImplicit;

                        // List messages in the INBOX folder
                        ImapMessageInfoCollection messageInfos = imapClient.ListMessages();

                        foreach (ImapMessageInfo messageInfo in messageInfos)
                        {
                            try
                            {
                                MailMessage fetchedMessage = imapClient.FetchMessage(messageInfo.UniqueId);
                                Console.WriteLine($"[IMAP] Subject: {fetchedMessage.Subject}");
                                Console.WriteLine($"[IMAP] From: {fetchedMessage.From}");
                                Console.WriteLine($"[IMAP] Date: {fetchedMessage.Date}");
                                Console.WriteLine();
                            }
                            catch (Exception fetchEx)
                            {
                                Console.Error.WriteLine($"Failed to fetch message UID {messageInfo.UniqueId}: {fetchEx.Message}");
                            }
                        }
                    }
                }
                catch (Exception imapEx)
                {
                    Console.Error.WriteLine($"IMAP client error: {imapEx.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
