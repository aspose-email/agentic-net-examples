using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;
using System;
using System.IO;

namespace AsposeEmailPstDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                const string pstFilePath = "storage.pst";
                const string outputDir = "output";

                // Ensure the output directory exists
                Directory.CreateDirectory(outputDir);

                // Ensure the PST file exists; create a minimal one if missing
                if (!File.Exists(pstFilePath))
                {
                    try
                    {
                        PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode);
                        Console.WriteLine($"Created new PST file at '{pstFilePath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                        return;
                    }
                }

                // Open the PST file
                using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
                {
                    // Display total items count in the store
                    long totalItemsCount = pst.Store.GetTotalItemsCount();
                    Console.WriteLine($"Total items count: {totalItemsCount}");

                    // Add a new message to the root folder to demonstrate an update
                    var newMessage = new MailMessage
                    {
                        From = new MailAddress("sender@example.com"),
                        Subject = "Demo Message",
                        Body = "This is a test message added to the PST."
                    };
                    newMessage.To.Add(new MailAddress("recipient@example.com"));

                    try
                    {
                        pst.RootFolder.AddMessage(MapiMessage.FromMailMessage(newMessage));
                        Console.WriteLine("Added a new message to the root folder.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to add message: {ex.Message}");
                    }

                    // Re‑enumerate subfolders and their messages
                    foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                    {
                        Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                        Console.WriteLine($"Total items: {folderInfo.ContentCount}");
                        Console.WriteLine($"Total unread items: {folderInfo.ContentUnreadCount}");

                        foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                        {
                            Console.WriteLine($"Subject: {messageInfo.Subject}");
                            try
                            {
                                // Extract as MapiMessage then convert to MailMessage
                                MapiMessage mapiMsg = pst.ExtractMessage(messageInfo);
                                MailMessage extracted = mapiMsg.ToMailMessage(new MailConversionOptions());

                                string subject = extracted.Subject ?? "Untitled";

                                // Build a safe file name
                                string safeFileName = string.Concat(subject.Split(Path.GetInvalidFileNameChars()));
                                if (string.IsNullOrWhiteSpace(safeFileName))
                                    safeFileName = "Untitled";

                                string msgPath = Path.Combine(outputDir, $"{safeFileName}.msg");
                                extracted.Save(msgPath);
                                Console.WriteLine($"Saved message to '{msgPath}'.");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to extract/save message: {ex.Message}");
                            }
                        }
                    }

                    // Also process messages directly in the root folder (if any)
                    foreach (MessageInfo rootMessageInfo in pst.RootFolder.EnumerateMessages())
                    {
                        Console.WriteLine($"[Root] Subject: {rootMessageInfo.Subject}");
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
