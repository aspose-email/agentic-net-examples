using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new Unicode PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode).Dispose();
                    Console.WriteLine($"Created placeholder PST file at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file with write access
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, true))
                {
                    // Add a test message to the root folder
                    MapiMessage testMessage = new MapiMessage("sender@example.com", "recipient@example.com", "Test Subject", "Test Body");
                    string entryId = pst.RootFolder.AddMessage(testMessage);
                    Console.WriteLine($"Added message with EntryId: {entryId}");

                    // Delete the previously added message
                    pst.DeleteItem(entryId);
                    Console.WriteLine($"Deleted message with EntryId: {entryId}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing PST file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
