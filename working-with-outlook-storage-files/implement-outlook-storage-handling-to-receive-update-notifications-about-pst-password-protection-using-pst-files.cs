using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine("Placeholder PST file created.");
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Error creating PST file: {createEx.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Subscribe to the ItemMoved event
                pst.ItemMoved += OnItemMoved;

                // Check if the PST is password protected
                if (pst.Store.IsPasswordProtected)
                {
                    Console.WriteLine("The PST file is password protected.");
                }
                else
                {
                    Console.WriteLine("The PST file is not password protected.");
                }

                // Keep the application alive briefly to demonstrate event handling (optional)
                Console.WriteLine("Press Enter to exit...");
                Console.ReadLine();

                // Unsubscribe from the event before disposing
                pst.ItemMoved -= OnItemMoved;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Event handler for ItemMoved
    private static void OnItemMoved(object sender, ItemMovedEventArgs e)
    {
        // Use the verified EntryId property
        Console.WriteLine($"Item moved. EntryId: {e.EntryId}");
    }
}
