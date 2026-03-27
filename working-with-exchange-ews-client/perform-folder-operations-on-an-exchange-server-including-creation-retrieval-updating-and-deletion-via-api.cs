using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace ExchangeFolderOperations
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Exchange server connection parameters
                string serverUri = "https://exchange.example.com/exchange/user@domain.com/";
                string username = "user";
                string password = "pass";

                // Create and use the ExchangeClient inside a using block
                using (Aspose.Email.Clients.Exchange.Dav.ExchangeClient client = new Aspose.Email.Clients.Exchange.Dav.ExchangeClient(serverUri, username, password))
                {
                    // Ensure the client connection is established safely
                    try
                    {
                        // Retrieve the Inbox folder URI from mailbox info
                        string inboxUri = client.MailboxInfo.InboxUri;
                        string newFolderName = "SampleFolder";

                        // -------------------------
                        // Create a new subfolder
                        // -------------------------
                        try
                        {
                            client.CreateFolder(inboxUri, newFolderName);
                            Console.WriteLine("Folder created: " + newFolderName);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine("CreateFolder error: " + ex.Message);
                        }

                        // -------------------------
                        // Retrieve folder information
                        // -------------------------
                        try
                        {
                            // Construct the full URI of the newly created folder
                            string newFolderUri = inboxUri + "/" + newFolderName;
                            Aspose.Email.Clients.Exchange.ExchangeFolderInfo folderInfo = client.GetFolderInfo(newFolderUri);
                            Console.WriteLine("Folder URI: " + folderInfo.Uri);
                            Console.WriteLine("Folder Display Name: " + folderInfo.DisplayName);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine("GetFolderInfo error: " + ex.Message);
                        }

                        // -------------------------
                        // List subfolders of the Inbox
                        // -------------------------
                        try
                        {
                            Aspose.Email.Clients.Exchange.ExchangeFolderInfoCollection subFolders = client.ListSubFolders(inboxUri);
                            Console.WriteLine("Subfolders of Inbox:");
                            foreach (Aspose.Email.Clients.Exchange.ExchangeFolderInfo info in subFolders)
                            {
                                Console.WriteLine("- " + info.DisplayName);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine("ListSubFolders error: " + ex.Message);
                        }

                        // -------------------------
                        // Delete the created folder
                        // -------------------------
                        try
                        {
                            string folderToDeleteUri = inboxUri + "/" + newFolderName;
                            client.DeleteFolder(folderToDeleteUri);
                            Console.WriteLine("Folder deleted: " + newFolderName);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine("DeleteFolder error: " + ex.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Exchange client error: " + ex.Message);
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