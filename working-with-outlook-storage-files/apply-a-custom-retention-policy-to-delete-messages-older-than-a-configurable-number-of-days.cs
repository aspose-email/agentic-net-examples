using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace RetentionPolicyExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Configurable retention period in days
                int retentionDays = 30;

                // Path to the PST file
                string pstPath = "sample.pst";

                // Ensure the PST file exists; create a minimal placeholder if missing
                if (!File.Exists(pstPath))
                {
                    try
                    {
                        // Create a new Unicode PST file
                        PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                        // Open the newly created PST to add a default Inbox folder
                        using (PersonalStorage createdPst = PersonalStorage.FromFile(pstPath))
                        {
                            // Add an "Inbox" folder under the root folder
                            createdPst.RootFolder.AddSubFolder("Inbox");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder PST file: {ex.Message}");
                        return;
                    }
                }

                // Open the PST file for processing
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Attempt to retrieve the Inbox folder; create it if it does not exist
                    FolderInfo inboxFolder;
                    try
                    {
                        inboxFolder = pst.RootFolder.GetSubFolder("Inbox");
                    }
                    catch
                    {
                        inboxFolder = pst.RootFolder.AddSubFolder("Inbox");
                    }

                    // Collect entry IDs of messages that exceed the retention period
                    List<byte[]> entriesToDelete = new List<byte[]>();

                    // Enumerate all messages in the Inbox folder
                    foreach (MessageInfo messageInfo in inboxFolder.EnumerateMessages())
                    {
                        try
                        {
                            // Extract the full MAPI message using the MessageInfo overload
                            MapiMessage message = pst.ExtractMessage(messageInfo);

                            // Determine the message date (prefer ClientSubmitTime, fallback to DeliveryTime)
                            DateTime messageDate = message.ClientSubmitTime;
                            if (messageDate == DateTime.MinValue)
                            {
                                messageDate = message.DeliveryTime;
                            }

                            // If the message is older than the retention threshold, mark for deletion
                            if ((DateTime.Now - messageDate).TotalDays > retentionDays)
                            {
                                entriesToDelete.Add(messageInfo.EntryId);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log extraction errors but continue processing other messages
                            Console.Error.WriteLine($"Error processing message ID {messageInfo.EntryIdString}: {ex.Message}");
                        }
                    }

                    // Delete the identified old messages
                    foreach (byte[] entryId in entriesToDelete)
                    {
                        try
                        {
                            inboxFolder.DeleteChildItem(entryId);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to delete message with entry ID: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Top-level exception handling
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }
    }
}
